            /*Observable0 observable0 = new Observable0();
            Observer1 observer1 = new Observer1();
            Observer2 observer2 = new Observer2();
            observable0.SomethingHappened += observer2.HandleEvent;
            observable0.SomethingHappened += observer1.HandleEvent;
            observable0.DoSomething();*/
            
    class Observable0
    {
        public event EventHandler SomethingHappened;
        public void DoSomething()
        {
            EventHandler handler = SomethingHappened;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

    class Observer1
    {
        public void HandleEvent(object sender, EventArgs args)
        {
            //
        }
    }

    class Observer2
    {
        public void HandleEvent(object sender, EventArgs args)
        {
            //
        }
    }
