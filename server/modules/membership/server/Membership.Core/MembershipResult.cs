using System.Collections.Generic;

namespace Membership.Core
{
	public class MembershipResult
	{
		public MembershipResult()
		{
			Errors = new List<string>();
		}

		public bool IsSuccess
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		public long UserId { get; set; }

		public string Token { get; set; }

		public List<string> Errors { get; set; } 
	}
}
