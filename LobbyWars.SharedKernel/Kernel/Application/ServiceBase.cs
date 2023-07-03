namespace LobbyWars.SharedKernel.Kernel.Application
{
    public abstract class ServiceBase
    {
        protected ServiceBase()
        {
            if (this.GetType().GetMethod("Invoke") == null)
                throw new InvalidOperationException("BaseClass subclasses should implement 'Invoke' method");
        }
    }
}
