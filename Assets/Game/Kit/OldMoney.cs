using System.Collections.Generic;

namespace ShallowWater.Game.Kit
{
    public static class OldMoney
    {
        private const int PenceInAShilling = 12;
        private const int ShillingsInAPound = 20;
        private const int PenceInAPound = PenceInAShilling * ShillingsInAPound;
        private const int None = 0;
        private const string PoundSign = "£";
        private const string ShillingSign = "s";
        private const string PennySign = "d";
        private const string Between = " ";
        private const string NotAPenny = "Not a penny";

        public static string Words(int pence)
        {
            var isEmpty = pence <= None;
            if (isEmpty) return NotAPenny;
            var pounds = pence / PenceInAPound;
            var shillings = pence % PenceInAPound / PenceInAShilling;
            var coppers = pence % PenceInAShilling;
            var words = new List<string>();
            var hasPounds = pounds > None;
            var hasShillings = shillings > None;
            var hasCoppers = coppers > None;
            if (hasPounds) words.Add(PoundSign + pounds);
            if (hasShillings) words.Add(shillings + ShillingSign);
            if (hasCoppers) words.Add(coppers + PennySign);
            return string.Join(Between, words);
        }
    }
}
