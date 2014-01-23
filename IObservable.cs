using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;

namespace ObservableDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            Observable0 observable0 = new Observable0();
            Observer1 observer1 = new Observer1();
            Observer2 observer2 = new Observer2();
            observable0.SomethingHappened += observer2.HandleEvent;
            observable0.SomethingHappened += observer1.HandleEvent;
            observable0.DoSomething();

            //将可观察序列的每个值投射到新表格中。
            IObservable<RoutedEventArgs> observable = Observable.Select<IEvent<TextChangedEventArgs>, RoutedEventArgs>
                //返回一个可观察序列，该序列包含基础 .NET Framework 事件的值
                (Observable.FromEvent<TextChangedEventHandler, TextChangedEventArgs>(h => new TextChangedEventHandler(h.Invoke),delegate(TextChangedEventHandler h)
                    {
                        this.SignInUserTextBox.TextChanged += h;
                    },
                    delegate(TextChangedEventHandler h)
                    {
                        this.SignInUserTextBox.TextChanged -= h;
                    }),
                    evt => new RoutedEventArgs()
                );

            IObservable<RoutedEventArgs> observable2 = Observable.Select<IEvent<RoutedEventArgs>, RoutedEventArgs>
                //返回一个可观察序列，该序列包含基础 .NET Framework 事件的值
                (Observable.FromEvent<RoutedEventHandler, RoutedEventArgs>(h => new RoutedEventHandler(h.Invoke), delegate(RoutedEventHandler h)
                    {
                        this.SignInPasswordBox.PasswordChanged += h;
                    }, delegate(RoutedEventHandler h)
                    {
                        this.SignInPasswordBox.PasswordChanged -= h;
                    }), 
                    evt => new RoutedEventArgs()
                );

            //监听事件观察者对象源
            ObservableExtensions.Subscribe(
                //返回一个可观察序列，它只包含独特的连续值。
               // Observable.DistinctUntilChanged(
                Observable.Select(
                    //将可观察序列的可观察序列合并到一个可观察序列中。
                    Observable.Merge<RoutedEventArgs>(observable, observable2),
                    evt => new User { Password = this.SignInPasswordBox.Password, Login = this.SignInUserTextBox.Text }
                ),
               // ),
                user => this.SignInButton.IsEnabled = !string.IsNullOrWhiteSpace(user.Login) && !string.IsNullOrWhiteSpace(user.Password)
            );
        }

    }

    public class User
    {
        public string Password { get; set; }
        public string Login { get; set; }
    }

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
}
