namespace Nocci
{
    public abstract class PuzzleObjectMachineListener : PuzzleObjectBehaviour
    {
        public PuzzleObjectMachine Machine { get; private set; }
        

        public override void OnBehaviourEnabled()
        {
            Machine = PuzzleObject.GetComponentInChildren<PuzzleObjectMachine>();
            Machine.OnStartRunning += OnMachineStart;
            Machine.OnStopRunning += OnMachineStop;
            
            if(Machine.IsRunning)
                OnMachineStart();
        }


        protected abstract void OnMachineStop();

        protected abstract void OnMachineStart();
    }
}