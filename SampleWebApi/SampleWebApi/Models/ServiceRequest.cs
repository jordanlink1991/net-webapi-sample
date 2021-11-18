using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApi.Models
{
	public enum ServiceRequestStatus
	{
		/// <summary>
		/// The default value - request has no status
		/// </summary>
		NotApplicable,
		/// <summary>
		/// The request is recently created
		/// </summary>
		Created,
		/// <summary>
		/// The request is in progress
		/// </summary>
		InProgress,
		/// <summary>
		/// The request has been completed
		/// </summary>
		Complete,
		/// <summary>
		/// The request has been canceled
		/// </summary>
		Canceled
	}

	public interface IServiceRequest
	{

	}

	[Table("ServiceRequest")]
	public class ServiceRequest : IServiceRequest
	{
		#region Attributes
		/// <summary>
		/// Unique ID of the service request
		/// </summary>
		[Key]
		[Required]
		public Guid ID { get; set; }
		/// <summary>
		/// Code of the building for the request
		/// </summary>
		public string BuildingCode { get; set; }
		/// <summary>
		/// Description of the requested service
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Current status of the requested service
		/// </summary>
		public ServiceRequestStatus CurrentStatus { get; set; }
		/// <summary>
		/// Name of the user who created the service
		/// </summary>
		public string CreatedBy { get; set; }
		/// <summary>
		/// Date the service was created
		/// </summary>
		public DateTime CreatedDate { get; set; }
		/// <summary>
		/// Name of the user who last updated the service
		/// </summary>
		public string LastModifiedBy { get; set; }
		/// <summary>
		/// Date the service was last updated
		/// </summary>
		public DateTime LastModifiedDate { get; set; }
		#endregion Attributes

		#region Constructors
		public ServiceRequest()
		{
			ID = new Guid();
			CurrentStatus = ServiceRequestStatus.NotApplicable;
			CreatedDate = DateTime.Now;
			LastModifiedDate = DateTime.Now;
		}
		#endregion Constructors
	}
}
