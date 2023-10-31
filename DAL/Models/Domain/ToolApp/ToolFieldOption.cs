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
    [Table("ToolFieldOption", Schema = "mobile")]
    public class ToolFieldOption
    {
        [Key]
        public int ToolFieldOptionId { get; set; }
        public int ToolFieldId { get; set; }
        //-----------Checkbox-1---------------
        public string CB11Label { get; set; }
        public bool CB11IsActive { get; set; } = false;
        public bool CB11Value { get; set; } = false;

        public string CB12Label { get; set; }
        public bool CB12IsActive { get; set; } = false;
        public bool CB12Value { get; set; } = false;

        public string CB13Label { get; set; }
        public bool CB13IsActive { get; set; } = false;
        public bool CB13Value { get; set; } = false;

        public string CB14Label { get; set; }
        public bool CB14IsActive { get; set; } = false;
        public bool CB14Value { get; set; } = false;

        public string CB15Label { get; set; }
        public bool CB15IsActive { get; set; } = false;
        public bool CB15Value { get; set; } = false;
        //-----------Checkbox-2---------------
        public string CB21Label { get; set; }
        public bool CB21IsActive { get; set; } = false;
        public bool CB21Value { get; set; } = false;

        public string CB22Label { get; set; }
        public bool CB22IsActive { get; set; } = false;
        public bool CB22Value { get; set; } = false;

        public string CB23Label { get; set; }
        public bool CB23IsActive { get; set; } = false;
        public bool CB23Value { get; set; } = false;

        public string CB24Label { get; set; }
        public bool CB24IsActive { get; set; } = false;
        public bool CB24Value { get; set; } = false;

        public string CB25Label { get; set; }
        public bool CB25IsActive { get; set; } = false;
        public bool CB25Value { get; set; } = false;
        //-----------Checkbox-3---------------
        public string CB31Label { get; set; }
        public bool CB31IsActive { get; set; } = false;
        public bool CB31Value { get; set; } = false;

        public string CB32Label { get; set; }
        public bool CB32IsActive { get; set; } = false;
        public bool CB32Value { get; set; } = false;

        public string CB33Label { get; set; }
        public bool CB33IsActive { get; set; } = false;
        public bool CB33Value { get; set; } = false;

        public string CB34Label { get; set; }
        public bool CB34IsActive { get; set; } = false;
        public bool CB34Value { get; set; } = false;

        public string CB35Label { get; set; }
        public bool CB35IsActive { get; set; } = false;
        public bool CB35Value { get; set; } = false;
        //-----------Checkbox-4---------------
        public string CB41Label { get; set; }
        public bool CB41IsActive { get; set; } = false;
        public bool CB41Value { get; set; } = false;

        public string CB42Label { get; set; }
        public bool CB42IsActive { get; set; } = false;
        public bool CB42Value { get; set; } = false;

        public string CB43Label { get; set; }
        public bool CB43IsActive { get; set; } = false;
        public bool CB43Value { get; set; } = false;

        public string CB44Label { get; set; }
        public bool CB44IsActive { get; set; } = false;
        public bool CB44Value { get; set; } = false;

        public string CB45Label { get; set; }
        public bool CB45IsActive { get; set; } = false;
        public bool CB45Value { get; set; } = false;
        //-----------Checkbox-5---------------
        public string CB51Label { get; set; }
        public bool CB51IsActive { get; set; } = false;
        public bool CB51Value { get; set; } = false;

        public string CB52Label { get; set; }
        public bool CB52IsActive { get; set; } = false;
        public bool CB52Value { get; set; } = false;

        public string CB53Label { get; set; }
        public bool CB53IsActive { get; set; } = false;
        public bool CB53Value { get; set; } = false;

        public string CB54Label { get; set; }
        public bool CB54IsActive { get; set; } = false;
        public bool CB54Value { get; set; } = false;

        public string CB55Label { get; set; }
        public bool CB55IsActive { get; set; } = false;
        public bool CB55Value { get; set; } = false;
        //-----------Checkbox-6---------------
        public string CB61Label { get; set; }
        public bool CB61IsActive { get; set; } = false;
        public bool CB61Value { get; set; } = false;

        public string CB62Label { get; set; }
        public bool CB62IsActive { get; set; } = false;
        public bool CB62Value { get; set; } = false;

        public string CB63Label { get; set; }
        public bool CB63IsActive { get; set; } = false;
        public bool CB63Value { get; set; } = false;

        public string CB64Label { get; set; }
        public bool CB64IsActive { get; set; } = false;
        public bool CB64Value { get; set; } = false;

        public string CB65Label { get; set; }
        public bool CB65IsActive { get; set; } = false;
        public bool CB65Value { get; set; } = false;
        //-----------Checkbox-7---------------
        public string CB71Label { get; set; }
        public bool CB71IsActive { get; set; } = false;
        public bool CB71Value { get; set; } = false;

        public string CB72Label { get; set; }
        public bool CB72IsActive { get; set; } = false;
        public bool CB72Value { get; set; } = false;

        public string CB73Label { get; set; }
        public bool CB73IsActive { get; set; } = false;
        public bool CB73Value { get; set; } = false;

        public string CB74Label { get; set; }
        public bool CB74IsActive { get; set; } = false;
        public bool CB74Value { get; set; } = false;

        public string CB75Label { get; set; }
        public bool CB75IsActive { get; set; } = false;
        public bool CB75Value { get; set; } = false;
        //-----------Checkbox-8---------------
        public string CB81Label { get; set; }
        public bool CB81IsActive { get; set; } = false;
        public bool CB81Value { get; set; } = false;

        public string CB82Label { get; set; }
        public bool CB82IsActive { get; set; } = false;
        public bool CB82Value { get; set; } = false;

        public string CB83Label { get; set; }
        public bool CB83IsActive { get; set; } = false;
        public bool CB83Value { get; set; } = false;

        public string CB84Label { get; set; }
        public bool CB84IsActive { get; set; } = false;
        public bool CB84Value { get; set; } = false;

        public string CB85Label { get; set; }
        public bool CB85IsActive { get; set; } = false;
        public bool CB85Value { get; set; } = false;
        //-----------Checkbox-9---------------
        public string CB91Label { get; set; }
        public bool CB91IsActive { get; set; } = false;
        public bool CB91Value { get; set; } = false;

        public string CB92Label { get; set; }
        public bool CB92IsActive { get; set; } = false;
        public bool CB92Value { get; set; } = false;

        public string CB93Label { get; set; }
        public bool CB93IsActive { get; set; } = false;
        public bool CB93Value { get; set; } = false;

        public string CB94Label { get; set; }
        public bool CB94IsActive { get; set; } = false;
        public bool CB94Value { get; set; } = false;

        public string CB95Label { get; set; }
        public bool CB95IsActive { get; set; } = false;
        public bool CB95Value { get; set; } = false;
        //-----------Radiobox-1---------------
        public string RB11Label { get; set; }
        public bool RB11IsActive { get; set; } = false;
        public bool RB11Value { get; set; } = false;

        public string RB12Label { get; set; }
        public bool RB12IsActive { get; set; } = false;
        public bool RB12Value { get; set; } = false;

        public string RB13Label { get; set; }
        public bool RB13IsActive { get; set; } = false;
        public bool RB13Value { get; set; } = false;

        public string RB14Label { get; set; }
        public bool RB14IsActive { get; set; } = false;
        public bool RB14Value { get; set; } = false;

        public string RB15Label { get; set; }
        public bool RB15IsActive { get; set; } = false;
        public bool RB15Value { get; set; } = false;
        //-----------Radiobox-2---------------
        public string RB21Label { get; set; }
        public bool RB21IsActive { get; set; } = false;
        public bool RB21Value { get; set; } = false;

        public string RB22Label { get; set; }
        public bool RB22IsActive { get; set; } = false;
        public bool RB22Value { get; set; } = false;

        public string RB23Label { get; set; }
        public bool RB23IsActive { get; set; } = false;
        public bool RB23Value { get; set; } = false;

        public string RB24Label { get; set; }
        public bool RB24IsActive { get; set; } = false;
        public bool RB24Value { get; set; } = false;

        public string RB25Label { get; set; }
        public bool RB25IsActive { get; set; } = false;
        public bool RB25Value { get; set; } = false;
        //-----------Radiobox-3---------------
        public string RB31Label { get; set; }
        public bool RB31IsActive { get; set; } = false;
        public bool RB31Value { get; set; } = false;

        public string RB32Label { get; set; }
        public bool RB32IsActive { get; set; } = false;
        public bool RB32Value { get; set; } = false;

        public string RB33Label { get; set; }
        public bool RB33IsActive { get; set; } = false;
        public bool RB33Value { get; set; } = false;

        public string RB34Label { get; set; }
        public bool RB34IsActive { get; set; } = false;
        public bool RB34Value { get; set; } = false;

        public string RB35Label { get; set; }
        public bool RB35IsActive { get; set; } = false;
        public bool RB35Value { get; set; } = false;
        //-----------Radiobox-4---------------
        public string RB41Label { get; set; }
        public bool RB41IsActive { get; set; } = false;
        public bool RB41Value { get; set; } = false;

        public string RB42Label { get; set; }
        public bool RB42IsActive { get; set; } = false;
        public bool RB42Value { get; set; } = false;

        public string RB43Label { get; set; }
        public bool RB43IsActive { get; set; } = false;
        public bool RB43Value { get; set; } = false;

        public string RB44Label { get; set; }
        public bool RB44IsActive { get; set; } = false;
        public bool RB44Value { get; set; } = false;

        public string RB45Label { get; set; }
        public bool RB45IsActive { get; set; } = false;
        public bool RB45Value { get; set; } = false;
        //-----------Radiobox-5---------------
        public string RB51Label { get; set; }
        public bool RB51IsActive { get; set; } = false;
        public bool RB51Value { get; set; } = false;

        public string RB52Label { get; set; }
        public bool RB52IsActive { get; set; } = false;
        public bool RB52Value { get; set; } = false;

        public string RB53Label { get; set; }
        public bool RB53IsActive { get; set; } = false;
        public bool RB53Value { get; set; } = false;

        public string RB54Label { get; set; }
        public bool RB54IsActive { get; set; } = false;
        public bool RB54Value { get; set; } = false;

        public string RB55Label { get; set; }
        public bool RB55IsActive { get; set; } = false;
        public bool RB55Value { get; set; } = false;
        //-----------Radiobox-6---------------
        public string RB61Label { get; set; }
        public bool RB61IsActive { get; set; } = false;
        public bool RB61Value { get; set; } = false;

        public string RB62Label { get; set; }
        public bool RB62IsActive { get; set; } = false;
        public bool RB62Value { get; set; } = false;

        public string RB63Label { get; set; }
        public bool RB63IsActive { get; set; } = false;
        public bool RB63Value { get; set; } = false;

        public string RB64Label { get; set; }
        public bool RB64IsActive { get; set; } = false;
        public bool RB64Value { get; set; } = false;

        public string RB65Label { get; set; }
        public bool RB65IsActive { get; set; } = false;
        public bool RB65Value { get; set; } = false;
        //-----------Radiobox-7---------------
        public string RB71Label { get; set; }
        public bool RB71IsActive { get; set; } = false;
        public bool RB71Value { get; set; } = false;

        public string RB72Label { get; set; }
        public bool RB72IsActive { get; set; } = false;
        public bool RB72Value { get; set; } = false;

        public string RB73Label { get; set; }
        public bool RB73IsActive { get; set; } = false;
        public bool RB73Value { get; set; } = false;

        public string RB74Label { get; set; }
        public bool RB74IsActive { get; set; } = false;
        public bool RB74Value { get; set; } = false;

        public string RB75Label { get; set; }
        public bool RB75IsActive { get; set; } = false;
        public bool RB75Value { get; set; } = false;
        //-----------Radiobox-8---------------
        public string RB81Label { get; set; }
        public bool RB81IsActive { get; set; } = false;
        public bool RB81Value { get; set; } = false;

        public string RB82Label { get; set; }
        public bool RB82IsActive { get; set; } = false;
        public bool RB82Value { get; set; } = false;

        public string RB83Label { get; set; }
        public bool RB83IsActive { get; set; } = false;
        public bool RB83Value { get; set; } = false;

        public string RB84Label { get; set; }
        public bool RB84IsActive { get; set; } = false;
        public bool RB84Value { get; set; } = false;

        public string RB85Label { get; set; }
        public bool RB85IsActive { get; set; } = false;
        public bool RB85Value { get; set; } = false;
        //-----------Radiobox-9---------------
        public string RB91Label { get; set; }
        public bool RB91IsActive { get; set; } = false;
        public bool RB91Value { get; set; } = false;

        public string RB92Label { get; set; }
        public bool RB92IsActive { get; set; } = false;
        public bool RB92Value { get; set; } = false;

        public string RB93Label { get; set; }
        public bool RB93IsActive { get; set; } = false;
        public bool RB93Value { get; set; } = false;

        public string RB94Label { get; set; }
        public bool RB94IsActive { get; set; } = false;
        public bool RB94Value { get; set; } = false;

        public string RB95Label { get; set; }
        public bool RB95IsActive { get; set; } = false;
        public bool RB95Value { get; set; } = false;
    }
}
