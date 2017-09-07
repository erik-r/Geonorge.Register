using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kartverket.Register.Models
{
    public abstract class RegisterItemV2
    {
        [Key]
        public Guid SystemId { get; set; }

        [Required]
        [Display(Name = "Navn:")]
        public string Name { get; set; }

        [Required]
        public string Seoname { get; set; }

        [Display(Name = "Beskrivelse:")]
        public string Description { get; set; }

        [ForeignKey("Submitter"), Required]
        public Guid SubmitterId { get; set; }
        [Display(Name = "Innsender:")]
        public virtual Organization Submitter { get; set; }

        [ForeignKey("Owner"), Required]
        public Guid OwnerId { get; set; }
        [Display(Name = "Eier:")]
        public virtual Organization Owner { get; set; }

        [Display(Name = "Dato innsendt:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateSubmitted { get; set; }

        [Display(Name = "Dato endret:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Modified { get; set; }

        [ForeignKey("Status"), Required]
        public string StatusId { get; set; } = "Submitted";
        [Display(Name = "Status:")]
        public virtual Status Status { get; set; }

        [ForeignKey("Register"), Required]
        public Guid RegisterId { get; set; }
        [Display(Name = "Register:")]
        public virtual Register Register { get; set; }

        protected RegisterItemV2() {
            SystemId = Guid.NewGuid();
        }

        public string DetailPageUrl()
        {
            return Register.GetObjectUrl() + "/" + Owner.seoname + "/" + Seoname;
        }

        public string ItemsByOwnerUrl()
        {
            return Register.GetObjectUrl() + "/" + Owner.seoname;
        }

        
    }
}//end namespace Datamodell