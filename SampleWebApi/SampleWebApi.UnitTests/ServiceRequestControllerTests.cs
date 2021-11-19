using System;
using Xunit;
using SampleWebApi.Controllers;
using SampleWebApi.Models;
using SampleWebApi.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SampleWebApi.UnitTests
{
	public class ServiceRequestControllerTests
	{
		#region Private Attributes
		/// <summary>
		/// Database Context used by the controller
		/// </summary>
		private DatabaseContext DatabaseContext { get; set; }
		/// <summary>
		/// Controller that is the subject of this class' tests
		/// </summary>
		private ServiceRequestController ServiceRequestController { get; set; }
		#endregion Private Attributes

		#region Constructors
		public ServiceRequestControllerTests()
		{
			var builder = new DbContextOptionsBuilder<DatabaseContext>()
				.UseSqlite("DataSource=:memory:", x => { });

			DatabaseContext = new DatabaseContext(builder.Options);
			ServiceRequestController = new ServiceRequestController(DatabaseContext);
		}
		#endregion Constructors

		#region Public Methods
		[Fact]
		public void Test1()
		{
		}
		#endregion Public Methods
	}
}
