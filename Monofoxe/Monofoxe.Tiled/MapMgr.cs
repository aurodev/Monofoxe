﻿using System;
using System.Collections.Generic;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled.MapStructure.Objects;


namespace Monofoxe.Tiled
{
	public static class MapMgr
	{
		/// <summary>
		/// Pool of all factories in all assemblies. Sorted by their tags.
		/// </summary>
		private static Dictionary<string, ITiledEntityFactory> _factoryPool;

		
		/// <summary>
		/// Initializes Tiled map loading stuff.
		/// HAS to be called in the beginning of the game, if you want to load Tiled maps.
		/// </summary>
		public static void Init()
		{
			InitFactoryPool();
		}



		/// <summary>
		/// Makes entity from Tiled temmplate using factory pool.
		/// </summary>
		public static Entity MakeEntity(TiledObject obj, Layer layer)
		{
			if (_factoryPool.ContainsKey(obj.Type))
			{
				return _factoryPool[obj.Type].Make(obj, layer);
			}
			return null;
		}


		/// <summary>
		/// Creates pool of all factories.
		/// </summary>
		private static void InitFactoryPool()
		{
			_factoryPool = new Dictionary<string, ITiledEntityFactory>();
			
			// Creating an instance of each.
			foreach(var type in GameMgr.Types)
			{
				if (typeof(ITiledEntityFactory).IsAssignableFrom(type.Value) && !type.Value.IsInterface)
				{
					var newFactory = (ITiledEntityFactory)Activator.CreateInstance(type.Value);
					_factoryPool.Add(newFactory.Tag, newFactory);
				}
			}
		}


	}
}
