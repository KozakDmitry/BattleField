using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DIContainer
{
    static DIContainer instance;
    readonly Dictionary<(Type type, string tag), DIRegistration> _registrations = new();
    readonly Dictionary<(Type type, string tag), DISubscribe> _subscribe = new();
    private readonly Dictionary<(Type type, string tag), List<DIWaiter>> _waiters = new();
    private CancellationTokenSource _currentCancellationToken;
    public static DIContainer Get()
    {
        instance ??= new();
        return instance;
    }

    public DIContainer()
    {
        _currentCancellationToken = new();
    }

    public void Register<T>(T instance, RegisterMode mode = RegisterMode.global)
        => Register(instance, null, mode);

    public void Register<T>(T instance, string tag, RegisterMode mode = RegisterMode.global)
    {
        var key = (typeof(T), tag);
        if (_registrations.ContainsKey(key))
        {
            Debug.LogWarning($"DI: Registration with tag '{tag}' for type {typeof(T)} already exists.");
            return;
        }

        _registrations[key] = new DIRegistration(instance, mode);

        if (_subscribe.TryGetValue(key, out var subscribe))
        {
            subscribe?.CallbackSubscribe?.Invoke(instance);
            _subscribe.Remove(key);
        }
        if (_waiters.TryGetValue(key, out var waiters))
        {
            var snapshot = waiters.ToArray();
            waiters.Clear();
            foreach (var tcs in snapshot)
            {
                tcs.awaiter.TrySetResult();
            }
        }
    }
    public void Resolve<T>(Action<T> callback)
        => Resolve(callback, null);

    public void Resolve<T>(Action<T> callback, string tag)
    {
        var key = (typeof(T), tag);
        if (_registrations.TryGetValue(key, out var registration))
        {
            if (registration.Instance != null)
                callback?.Invoke((T)registration.Instance);
        }
        else
        {
            Action<object> wrapper = obj => callback?.Invoke((T)obj);
            if (!_subscribe.ContainsKey(key))
            {
                _subscribe[key] = new DISubscribe
                {
                    Type = typeof(T),
                    CallbackSubscribe = wrapper
                };
            }
            else
            {
                _subscribe[key].CallbackSubscribe += wrapper;
            }
        }
    }
    private void RemoveWaiter((Type, string) key, DIWaiter waiter)
    {
        if (_waiters.TryGetValue(key, out var waiters))
        {
            waiters.Remove(waiter);
            if (waiters.Count == 0)
            {
                _waiters.Remove(key);
            }
        }
    }
    public async UniTask<T> ResolveAsync<T>(string tag = null, RegisterMode mode = RegisterMode.scene)
    {
        var key = (typeof(T), tag);

        if (_registrations.TryGetValue(key, out var registration) && registration.Instance != null)
        {
            return (T)registration.Instance;
        }

        var waiter = new DIWaiter(mode, new UniTaskCompletionSource());
        waiter.token = _currentCancellationToken.Token.Register(() =>
        {
            if (waiter.mode == RegisterMode.scene)
            {
                waiter.awaiter.TrySetCanceled(_currentCancellationToken.Token);
                RemoveWaiter(key, waiter);
            }
        });

        if (!_waiters.ContainsKey(key))
        {
            _waiters[key] = new List<DIWaiter>();
        }

        _waiters[key].Add(waiter);
        await waiter.awaiter.Task;

        waiter.token.Dispose();
        RemoveWaiter(key, waiter);

        return (T)_registrations[key].Instance;
    }

    public T ResolveSync<T>()
        => ResolveSync<T>(null);

    public T ResolveSync<T>(string tag)
    {
        var key = (typeof(T), tag);
        if (_registrations.TryGetValue(key, out var registration) && registration.Instance != null)
            return (T)registration.Instance;

        return default;
    }

    public void Remove(Type type)
        => Remove(type, null);

    public void Remove(Type type, string tag)
    {
        var key = (type, tag);
        _registrations.Remove(key);
    }

    public void RemoveAllForThisScene()
    {
        _currentCancellationToken?.Cancel();
        _currentCancellationToken = new();
        var keysToRemove = new List<(Type type, string tag)>();
        foreach (var kvp in _registrations)
        {
            if (kvp.Value.Instance == null || kvp.Value.mode == RegisterMode.scene)
            {
                keysToRemove.Add(kvp.Key);

            }
        }
        foreach (var key in keysToRemove)
            _registrations.Remove(key);
    }
}

public enum RegisterMode
{
    global,
    scene
}

public static class DI
{

    public static void Register<T>(T instance, RegisterMode mode = RegisterMode.global)
        => DIContainer.Get().Register(instance, mode);

    public static void Register<T>(T instance, string tag, RegisterMode mode = RegisterMode.global)
        => DIContainer.Get().Register(instance, tag, mode);

    public static void Resolve<T>(Action<T> callback)
        => DIContainer.Get().Resolve(callback);

    public static void Resolve<T>(Action<T> callback, string tag)
        => DIContainer.Get().Resolve(callback, tag);
    public static async UniTask<T> ResolveAsync<T>(string tag = null)
    => await DIContainer.Get().ResolveAsync<T>(tag);

    public static T ResolveSync<T>()
        => DIContainer.Get().ResolveSync<T>();

    public static T ResolveSync<T>(string tag)
        => DIContainer.Get().ResolveSync<T>(tag);

    public static void Remove(Type type)
        => DIContainer.Get().Remove(type);

    public static void Remove(Type type, string tag)
        => DIContainer.Get().Remove(type, tag);

    public static void RemoveAllForThisScene()
        => DIContainer.Get().RemoveAllForThisScene();
}