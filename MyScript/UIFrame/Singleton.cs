using UnityEngine;
using System.Collections;

//单例的泛型
//Singleton<T>泛型
//: MonoBehaviour表示要继承MonoBehaviour类
//where T : MonoBehaviour 泛型约束 ，约束T需要是MonoBehaviour类型
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
    private static T instance;  //私有的实例   
    public static T Instance   //单例的实例获取的入口
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        //this as T只有T添加了约束之后，才可以使用as转换
        instance = this as T;
    }
}
