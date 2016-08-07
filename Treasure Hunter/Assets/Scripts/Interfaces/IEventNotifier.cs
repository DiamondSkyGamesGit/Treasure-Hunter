using UnityEngine;
using System.Collections;
using System;

namespace MyEventSystem {

    public class MyEventArgs : EventArgs
    {
      
    }

    public interface IEventNotifier {

        MyEventArgs myEventArgs { get; set; }
    }

}
