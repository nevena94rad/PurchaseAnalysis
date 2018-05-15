using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Prepare
{
    public interface I_ARIMAPrepare : I_PrepareDisplay
    {
        ARIMAData ARIMAprepare(int date);
        List<string> ARIMAreadAllCustomers();
        List<ARIMAItemData> ARIMAreadAllItems(string custNo);
        int ARIMAgetStartDate(string custNo, string itemNo, bool isGPI);
        int ARIMAgetEndDate(string custNo, string itemNo, bool isGPI);
        List<ARIMAConsumptionData> ARIMAgetCustomerConsumption(string custNo, string itemNo, int start, int end, bool isGPI);
        List<ARIMAConsumptionData> ARIMAgetGlobalConsumption(string itemNo, int start, int end);
        List<ARIMAConsumptionData> ARIMATransformQuantityData(List<ARIMAConsumptionData> quantity);
        int ARIMAgetPurchaseQuantity(string custNo, string item, int date, bool isGPI);
        string ARIMAgetScriptPath();

    }
}
