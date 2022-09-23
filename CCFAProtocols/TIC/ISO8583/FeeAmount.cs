namespace CCFAProtocols.TIC.ISO8583
{
    public class AcquirerFeeAmount
    {
        /// <summary>
        ///     <value>
        ///         <para>'D'-withdraw, true</para>
        ///         <para>'C'-append, false</para>
        ///     </value>
        /// </summary>
        public char _isWithdraw;

        public uint Amount;

        /// '
        /// <inheritdoc cref="_isWithdraw" />
        public bool IsWithdraw
        {
            get => _isWithdraw == 'D';
            set => _isWithdraw = value ? 'D' : 'C';
        }

        public override string ToString()
        {
            var str = "FeeAmount:";
            foreach (var field in typeof(AcquirerFeeAmount).GetFields())
                str += $"\n\t\t{field.Name}: {field.GetValue(this)}";

            return str;
        }
    }
}