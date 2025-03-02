using System;
using Obi;
using UnityEngine;

namespace Nocci
{
    public class PuzzleCableEnd : PuzzleObjectBehaviour
    {
        public PowerConnector connectedConnector;
        public ObiParticleAttachment ropeAttach;
        
        public event Action<(PuzzleCableEnd,PowerConnector, PowerConnector)> OnConnectionChanged = delegate { }; 
        
        public void Connect(PowerConnector connector)
        {
           var lastConnection = connectedConnector;
           connectedConnector = connector;
           ropeAttach.attachmentType = ObiParticleAttachment.AttachmentType.Static;
           
           OnConnectionChanged.Invoke((this, lastConnection, connectedConnector));
        }
        
        public void Disconnect()
        {
            var lastConnection = connectedConnector;
            connectedConnector = null;
            ropeAttach.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
            
            OnConnectionChanged.Invoke((this, lastConnection, null));
        }
    }
}