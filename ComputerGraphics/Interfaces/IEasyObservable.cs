namespace ComputerGraphics.Interfaces
{
    internal interface IEasyObservable<T>
    {
        void AddObserver(IEasyObserver<T> observer);
        void RemoveObserver(IEasyObserver<T> observer);
        void NotifyObservers();
    }
}
