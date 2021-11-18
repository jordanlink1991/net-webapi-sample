using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleWebApi
{
	/// <summary>
	/// Starting point for the API when using IIS Express
	/// </summary>
	public class Launcher
	{
		/// <summary>
		/// Entry point for the solution
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			// Create the host builder
			IHostBuilder builder = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(
				webBuilder => {
					webBuilder.UseStartup<ApiStarter>();
			}); 

			// Run the builder
			builder.Build().Run();
		}
	}
}
