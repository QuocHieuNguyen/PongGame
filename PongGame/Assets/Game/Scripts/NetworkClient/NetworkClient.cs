using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameCore {
    public class NetworkClient {

        public virtual void Invoke(string key, object value){

        }
        public virtual void Listen(string key, Action<object> value){

        }
        public virtual void StopListen(string key, Action<object> value){

        }
    }
}