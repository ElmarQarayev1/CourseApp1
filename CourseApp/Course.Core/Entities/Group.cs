using System;
namespace Course.Core.Entities
{
	public class Group:AuditEntity
	{
        public string No { get; set; }

        public byte Limit { get; set; }

        public List<Student> Students { get; set; }
    }
}

