namespace Utilities
{ 
    public abstract class CustomPair<TValue, UMirror> where TValue : struct where UMirror : struct
    {
        #region fields

        public TValue Value { get; }
        public UMirror Mirror { get; }

        #endregion

        #region constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mirror"></param>
        protected CustomPair(TValue value, UMirror mirror)
        {
            Value = value;
            Mirror = mirror;
        }

        #endregion
    }
}