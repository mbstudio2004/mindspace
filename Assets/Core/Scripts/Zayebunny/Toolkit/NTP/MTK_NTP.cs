using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Nocci.Zayebunny.Extensions;
using UnityEngine;

namespace Nocci.Zayebunny.NTP
{
    public class MTK_NTP : Singleton<MTK_NTP>
    {
        public delegate void TimeInitialized();

        public delegate void TimeRequestError();

        /// <summary>
        ///     Address to server supporting NTP (preferred 'pool.ntp.org' which will automatically redirect to best one).
        /// </summary>
        private const string NtpServer = "pool.ntp.org";

        /// <summary>
        ///     NTP port on server. In most cases is 123.
        /// </summary>
        private const int NtpServerPort = 123;

        /// <summary>
        ///     Buffer size transfered between client <=> server.
        /// </summary>
        private const int NtpBufferSize = 48;

        /// <summary>
        ///     Buffer header used for protocol configuration (Eg.: 0x23: 00...(0 - no warnings)..100..(4 - protocol
        ///     version)..011(3 - client mode)).
        /// </summary>
        private const int NtpHeader = 0x23;

        /// <summary>
        ///     Request timeout.
        /// </summary>
        private const float NtpTimeout = 3f;

        private bool _isTimeInitialized;

        /// <summary>
        ///     Stored last system uptime.
        /// </summary>
        private float _lastSytemUptime;

        /// <summary>
        ///     Date from server on DateTime.UtcNow.
        /// </summary>
        private DateTime _ntpDate;

        private byte[] _recivedTimeData;
        private volatile bool _threadRunning;
        private Thread _timeRequestThread;

        private UdpClient _udpClient;

        /// <summary>
        ///     Last update time in seconds since game started.
        /// </summary>
        private float _updateTime;

        /// <summary>
        ///     Date recived from server or machine UTC time.
        /// </summary>
        public DateTime Date
        {
            get
            {
                _lastSytemUptime = GetSystemUptime();
                // Add elapsed seconds from last time update
                return _ntpDate.AddSeconds(_lastSytemUptime - _updateTime);

                // nmg ga recreate jer ne znam kad se desi, totalno je random 
            }
        }

        public bool IsTimeInitialized
        {
            get => _isTimeInitialized;
            private set
            {
                if (_isTimeInitialized == value) return;
                _isTimeInitialized = value;
                if (IsTimeInitialized && OnTimeInitialized != null)
                    OnTimeInitialized();
            }
        }

        /// <summary>
        ///     Clear data and get time from server
        /// </summary>
        public void Reset()
        {
            IsTimeInitialized = false;

            Dispose();
            RefreshNetworkTimeAsync();
        }

        private void OnApplicationPause(bool isPaused)
        {
            // If device is restarted system uptime will reset
            if (IsTimeInitialized && !isPaused && GetSystemUptime() < _lastSytemUptime && !_threadRunning)
                RefreshNetworkTimeAsync();
        }

        private void OnApplicationQuit()
        {
            Dispose();
        }

        /// <summary>
        ///     Dispached when time is successfuly recived from server.
        /// </summary>
        public event TimeInitialized OnTimeInitialized;

        /// <summary>
        ///     Dispached when cannot recive time from server.
        /// </summary>
        public event TimeRequestError OnTimeRequestError;


        public void Setup()
        {
            DontDestroyOnLoad(gameObject);
            if (!_threadRunning)
                RefreshNetworkTimeAsync();
        }

        private void Request()
        {
            // Get potentialy free port and machine adress
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // Clear buffer to know if time is successfuly updated
            _recivedTimeData = null;

            try // Sockets sometimes stays open even if not used. In that case we need to catch Exeption.
            {
                _udpClient = new UdpClient(ipEndPoint);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Cannot open UDP port: {e.Message}");
                _threadRunning = false;
                return;
            }

            var sendData = new byte[NtpBufferSize];
            sendData[0] = NtpHeader;

            try // If host is unreachable we need to catch exeption.
            {
                _udpClient.Send(sendData, sendData.Length, NtpServer, NtpServerPort);
                _recivedTimeData = _udpClient.Receive(ref ipEndPoint);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Cannot get time via NTP: {e.Message}");
            }

            _udpClient.Close();

            _threadRunning = false;
        }

        private IEnumerator WaitForRequest()
        {
            var startWaitTime = Time.realtimeSinceStartup;
            while (_threadRunning && Time.realtimeSinceStartup - startWaitTime < NtpTimeout) yield return null;

            var date = new DateTime(1900, 1, 1);
            if (_threadRunning || _recivedTimeData == null || _recivedTimeData.Length == 0)
            {
                // If cannot get time from server, use device time.
                SetTime(DateTime.UtcNow);

                UnityEngine.Debug.LogWarning("NTP is using device time.");

                OnTimeRequestError?.Invoke();
            }
            else
            {
                // NTP server returns seconds since 01.01.1900
                var high = (double)BitConverter.ToUInt32(
                    new[] { _recivedTimeData[43], _recivedTimeData[42], _recivedTimeData[41], _recivedTimeData[40] },
                    0);
                var low = (double)BitConverter.ToUInt32(
                    new[] { _recivedTimeData[47], _recivedTimeData[46], _recivedTimeData[45], _recivedTimeData[44] },
                    0);
                date = date.AddSeconds(high + low / uint.MaxValue);

                SetTime(date);
            }
        }

        /// <summary>
        ///     Clear data and get time from server
        /// </summary>
        public void Dispose()
        {
            _udpClient?.Close();
        }

        /// <summary>
        ///     Recive time from server or sets machine time as current if fail
        /// </summary>
        public void RefreshNetworkTimeAsync()
        {
            _threadRunning = true;
            StartCoroutine(WaitForRequest());

            _timeRequestThread = new Thread(Request);
            _timeRequestThread.Start();
        }

        /// <summary>
        ///     Manualy set current time
        /// </summary>
        public void SetTime(DateTime time)
        {
            _updateTime = GetSystemUptime();
            _ntpDate = time;
            IsTimeInitialized = true;
        }

        /// <summary>
        ///     Returns system uptime in seconds.
        ///     Notice that on iOS Time.realtimeSinceStartup is not correct after device suspension.
        ///     In that case is necessary to get e.g. kernel uptime instead.
        /// </summary>
        private float GetSystemUptime()
        {
            return Time.realtimeSinceStartup;
        }
    }
}