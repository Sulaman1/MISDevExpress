using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Models.Domain.ToolApp
{
    [Table("ToolField", Schema = "mobile")]
    public class ToolDetail
    {
        [Key]
        public int ToolFieldId { get; set; }
        public int ToolId { get; set; }
        //-----------TextBox----------------------
        public string TB1Label { get; set; }
        public bool TB1IsActive { get; set; } = false;
        public string TB1Value { get; set; }
        public bool TB1IsRick { get; set; } = false;
        public int TB1OrderNo { get; set; }

        public string TB2Label { get; set; }
        public bool TB2IsActive { get; set; } = false;
        public string TB2Value { get; set; }
        public bool TB2IsRick { get; set; } = false;
        public int TB2OrderNo { get; set; }

        public string TB3Label { get; set; }
        public bool TB3IsActive { get; set; } = false;
        public string TB3Value { get; set; }
        public bool TB3IsRick { get; set; } = false;
        public int TB3OrderNo { get; set; }

        public string TB4Label { get; set; }
        public bool TB4IsActive { get; set; } = false;
        public string TB4Value { get; set; }
        public bool TB4IsRick { get; set; } = false;
        public int TB4OrderNo { get; set; }

        public string TB5Label { get; set; }
        public bool TB5IsActive { get; set; } = false;
        public string TB5Value { get; set; }
        public bool TB5IsRick { get; set; } = false;
        public int TB5OrderNo { get; set; }

        public string TB6Label { get; set; }
        public bool TB6IsActive { get; set; } = false;
        public string TB6Value { get; set; }
        public bool TB6IsRick { get; set; } = false;
        public int TB6OrderNo { get; set; }

        public string TB7Label { get; set; }
        public bool TB7IsActive { get; set; } = false;
        public string TB7Value { get; set; }
        public bool TB7IsRick { get; set; } = false;
        public int TB7OrderNo { get; set; }

        public string TB8Label { get; set; }
        public bool TB8IsActive { get; set; } = false;
        public string TB8Value { get; set; }
        public bool TB8IsRick { get; set; } = false;
        public int TB8OrderNo { get; set; }

        public string TB9Label { get; set; }
        public bool TB9IsActive { get; set; } = false;
        public string TB9Value { get; set; }
        public bool TB9IsRick { get; set; } = false;
        public int TB9OrderNo { get; set; }
        //---------------Pictures-------------------
        public string Pic1Label { get; set; }
        public bool Pic1IsActive { get; set; } = false;
        public string Pic1Value { get; set; }
        public int Pic1OrderNo { get; set; }

        public string Pic2Label { get; set; }
        public bool Pic2IsActive { get; set; } = false;
        public string Pic2Value { get; set; }
        public int Pic2OrderNo { get; set; }

        public string Pic3Label { get; set; }
        public bool Pic3IsActive { get; set; } = false;
        public string Pic3Value { get; set; }
        public int Pic3OrderNo { get; set; }

        public string Pic4Label { get; set; }
        public bool Pic4IsActive { get; set; } = false;
        public string Pic4Value { get; set; }
        public int Pic4OrderNo { get; set; }

        public string Pic5Label { get; set; }
        public bool Pic5IsActive { get; set; } = false;
        public string Pic5Value { get; set; }
        public int Pic5OrderNo { get; set; }

        public string Pic6Label { get; set; }
        public bool Pic6IsActive { get; set; } = false;
        public string Pic6Value { get; set; }
        public int Pic6OrderNo { get; set; }

        public string Pic7Label { get; set; }
        public bool Pic7IsActive { get; set; } = false;
        public string Pic7Value { get; set; }
        public int Pic7OrderNo { get; set; }

        public string Pic8Label { get; set; }
        public bool Pic8IsActive { get; set; } = false;
        public string Pic8Value { get; set; }
        public int Pic8OrderNo { get; set; }

        public string Pic9Label { get; set; }
        public bool Pic9IsActive { get; set; } = false;
        public string Pic9Value { get; set; }
        public int Pic9OrderNo { get; set; }
        //-------------DateTime----------------
        public string DT1Label { get; set; }
        public bool Dt1IsActive { get; set; } = false;
        public DateTime DT1Value { get; set; } = DateTime.Today;
        public int DT1OrderNo { get; set; }

        public string DT2Label { get; set; }
        public bool Dt2IsActive { get; set; } = false;
        public DateTime DT2Value { get; set; } = DateTime.Today;
        public int DT2OrderNo { get; set; }

        public string DT3Label { get; set; }
        public bool Dt3IsActive { get; set; } = false;
        public DateTime DT3Value { get; set; } = DateTime.Today;
        public int DT3OrderNo { get; set; }

        public string DT4Label { get; set; }
        public bool Dt4IsActive { get; set; } = false;
        public DateTime DT4Value { get; set; } = DateTime.Today;
        public int DT4OrderNo { get; set; }

        public string DT5Label { get; set; }
        public bool Dt5IsActive { get; set; } = false;
        public DateTime DT5Value { get; set; } = DateTime.Today;
        public int DT5OrderNo { get; set; }
        //------------CheckBox----------------
        public string CB1Label { get; set; }
        public bool CB1IsActive { get; set; } = false;        
        public int CB1OrderNo { get; set; }

        public string CB2Label { get; set; }
        public bool CB2IsActive { get; set; } = false;
        public int CB2OrderNo { get; set; }

        public string CB3Label { get; set; }
        public bool CB3IsActive { get; set; } = false;
        public int CB3OrderNo { get; set; }

        public string CB4Label { get; set; }
        public bool CB4IsActive { get; set; } = false;
        public int CB4OrderNo { get; set; }

        public string CB5Label { get; set; }
        public bool CB5IsActive { get; set; } = false;
        public int CB5OrderNo { get; set; }

        public string CB6Label { get; set; }
        public bool CB6IsActive { get; set; } = false;
        public int CB6OrderNo { get; set; }

        public string CB7Label { get; set; }
        public bool CB7IsActive { get; set; } = false;
        public int CB7OrderNo { get; set; }

        public string CB8Label { get; set; }
        public bool CB8IsActive { get; set; } = false;
        public int CB8OrderNo { get; set; }

        public string CB9Label { get; set; }
        public bool CB9IsActive { get; set; } = false;
        public int CB9OrderNo { get; set; }
        //-----------Radiobox------------------------
        public string RB1Label { get; set; }
        public bool RB1IsActive { get; set; } = false;
        public int RB1OrderNo { get; set; }

        public string RB2Label { get; set; }
        public bool RB2IsActive { get; set; } = false;
        public int RB2OrderNo { get; set; }

        public string RB3Label { get; set; }
        public bool RB3IsActive { get; set; } = false;
        public int RB3OrderNo { get; set; }

        public string RB4Label { get; set; }
        public bool RB4IsActive { get; set; } = false;
        public int RB4OrderNo { get; set; }

        public string RB5Label { get; set; }
        public bool RB5IsActive { get; set; } = false;
        public int RB5OrderNo { get; set; }

        public string RB6Label { get; set; }
        public bool RB6IsActive { get; set; } = false;
        public int RB6OrderNo { get; set; }

        public string RB7Label { get; set; }
        public bool RB7IsActive { get; set; } = false;
        public int RB7OrderNo { get; set; }

        public string RB8Label { get; set; }
        public bool RB8IsActive { get; set; } = false;
        public int RB8OrderNo { get; set; }

        public string RB9Label { get; set; }
        public bool RB9IsActive { get; set; } = false;
        public int RB9OrderNo { get; set; }
    }
}
