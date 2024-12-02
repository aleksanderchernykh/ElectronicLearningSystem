// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ElectronicLearningSystemKafka.Common.Models
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	public partial class Email : ISpecificRecord
	{
		public static Schema _SCHEMA = Schema.Parse(@"{""type"":""record"",""name"":""Email"",""namespace"":""ElectronicLearningSystemKafka.Common.Models"",""fields"":[{""name"":""Recipients"",""type"":{""type"":""array"",""items"":""string""}},{""name"":""Text"",""type"":""string""},{""name"":""Subject"",""type"":""string""},{""name"":""Files"",""type"":[""null"",{""type"":""array"",""items"":""string""}]}]}");
		private IList<System.String> _Recipients;
		private string _Text;
		private string _Subject;
		private IList<System.String> _Files;
		public virtual Schema Schema
		{
			get
			{
				return Email._SCHEMA;
			}
		}
		public IList<System.String> Recipients
		{
			get
			{
				return this._Recipients;
			}
			set
			{
				this._Recipients = value;
			}
		}
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				this._Text = value;
			}
		}
		public string Subject
		{
			get
			{
				return this._Subject;
			}
			set
			{
				this._Subject = value;
			}
		}
		public IList<System.String> Files
		{
			get
			{
				return this._Files;
			}
			set
			{
				this._Files = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.Recipients;
			case 1: return this.Text;
			case 2: return this.Subject;
			case 3: return this.Files;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.Recipients = (IList<System.String>)fieldValue; break;
			case 1: this.Text = (System.String)fieldValue; break;
			case 2: this.Subject = (System.String)fieldValue; break;
			case 3: this.Files = (IList<System.String>)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
