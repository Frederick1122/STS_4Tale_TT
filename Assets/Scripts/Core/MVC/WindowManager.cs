using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowManager : MonoBehaviour
{
    protected Dictionary<Type, IUiController> _controllers = new();

    protected abstract void AddControllers();

    public virtual void Init()
    {
        AddControllers();
        InitAll();
        HideAll();
    }

    private void InitAll()
    {
        foreach (IUiController controller in _controllers.Values)
            controller.Init();
    }

    private void HideAll()
    {
        foreach (KeyValuePair<Type, IUiController> controllerPair in _controllers)
            controllerPair.Value.Hide();
    }

    public IUiController ShowWindow(Type T, bool hideOtherWindows = true)
    {        
        if (hideOtherWindows)
            HideAll();

        IUiController controller = _controllers[T];
        controller.Show();
        return controller;
    }

    public void HideWindow(Type T)
    {
        _controllers[T].Hide();
    }
}