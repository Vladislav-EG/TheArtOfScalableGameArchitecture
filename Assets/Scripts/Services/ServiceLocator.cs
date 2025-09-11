using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class ServiceLocator
{
	private static readonly Dictionary<Type, object> _services = new();

	// Регистрация сервиса
	public static void Register<T>(T service)
	{
		var type = typeof(T);
		if (_services.ContainsKey(type))
			_services[type] = service;
		else
			_services.Add(type, service);
	}
	
	public static void Register(object service, Type type)
    {
        if (_services.ContainsKey(type))
            _services[type] = service;
        else
            _services.Add(type, service);
    }

	// Получение сервиса
	public static T Get<T>()
	{
		var type = typeof(T);
		if (_services.TryGetValue(type, out var service))
			return (T)service;

		throw new Exception($"Сервис {type} не зарегистрирован в ServiceLocator");
	}

	public static float Count() => _services.Count;

	// Очистка (например при смене сцены)
	public static void Clear() => _services.Clear();
}
