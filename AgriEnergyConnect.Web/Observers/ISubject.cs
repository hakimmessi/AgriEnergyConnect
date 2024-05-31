﻿namespace AgriEnergyConnect.Web.Observers
{
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(string message);
    }

}