using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SampleWebApi.Models;

namespace SampleWebApi.DAL
{
	/// <summary>
	/// Interface for database contexts used by the API
	/// </summary>
	public interface IDatabaseContext
	{
		#region Attributes
		/// <summary>
		/// List of service requests in the database
		/// </summary>
		DbSet<ServiceRequest> ServiceRequests { get; set; }
		#endregion Attributes
	}

	/// <summary>
	/// Represents the database context within the API
	/// </summary>
	public class DatabaseContext : DbContext, IDatabaseContext
	{
		#region Attributes
		/// <summary>
		/// List of service requests in the database
		/// </summary>
		public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }
		#endregion Attributes

		#region Constructors
		public DatabaseContext()
			: base()
		{
		}

		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
		}
		#endregion Constructors
	}
}
