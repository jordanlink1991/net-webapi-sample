using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SampleWebApi.DAL
{
	public interface IDatabaseContext
	{

	}

	public class DatabaseContext : DbContext, IDatabaseContext
	{
		#region Attributes
		#endregion Attributes

		#region Constructors
		public DatabaseContext()
			: base()
		{
		}
		#endregion Constructors
	}
}
