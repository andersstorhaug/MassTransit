namespace BusDriver.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Magnum.CommandLineParser;

	public static class Extens
	{
		public static string GetDefinition(this IEnumerable<ICommandLineElement> elements, string key)
		{
			return elements.OfType<IDefinitionElement>()
				.Where(x => x.Key == key)
				.Select(x => x.Value)
				.Single();
		}

		public static bool GetSwitch(this IEnumerable<ICommandLineElement> elements, char key)
		{
			return elements.OfType<ISwitchElement>()
				.Where(x => x.Key == key.ToString())
				.Select(x => true)
				.SingleOrDefault();
		}

		public static T GetDefinition<T>(this IEnumerable<ICommandLineElement> elements, string key,
		                                 Func<string, T> converter)
		{
			return elements.OfType<IDefinitionElement>()
				.Where(x => x.Key == key)
				.Select(x => converter(x.Value))
				.Single();
		}
	}
}