using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class CustomDoubleArray<TValue, UMirror> where TValue : struct where UMirror : unmanaged
    {
        #region fields
        
        private TValue[] _values;
        private UMirror[] _mirrors;

        #endregion
        
        #region constructors
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="mirrors"></param>
        public CustomDoubleArray(TValue[] values, UMirror[] mirrors)
        {
            _values = values;
            _mirrors = mirrors;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mirror"></param>
        public CustomDoubleArray(TValue value, UMirror mirror)
        {
            _values = new[] { value };
            _mirrors = new[] { mirror };
        }
        
        /// <summary>
        /// 
        /// </summary>
        public CustomDoubleArray()
        {
            _values = new TValue[] { };
            _mirrors = new UMirror[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pairs"></param>
        public CustomDoubleArray(IReadOnlyList<CustomPair<TValue, UMirror>> pairs)
        {
            //Values inits
            _values = new TValue[pairs.Count];
            for (var i = 0; i < pairs.Count; i++)
            {
                _values[i] = pairs[i].Value;
            }
            //Mirrors inits
            _mirrors = new UMirror[pairs.Count];
            for (var i = 0; i < pairs.Count; i++)
            {
                _mirrors[i] = pairs[i].Mirror;
            }
        }
        
        #endregion
        
        #region methodes
        
        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        /// <exception cref="CustomExceptions.CustomException"></exception>
        public TValue GetValue(UMirror origin)
        {
            for (var i = 0; i < _mirrors.Length; i++)
            {
                if (_mirrors[i].Equals(origin))
                    return _values[i];
            }

            throw new CustomExceptions.CustomException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originMirror"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(UMirror originMirror, out TValue value)
        {
            value = default;
            for (var i = 0; i < _mirrors.Length; i++)
            {
                if (!_mirrors[i].Equals(originMirror))
                    continue;
                
                value = _values[i];
                return true;
            }
            Debug.Log("Unable to retrieve value from mirror");
            return false;
        }
        
        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        /// <exception cref="CustomExceptions.CustomException"></exception>
        public UMirror GetMirror(TValue origin)
        {
            for (var i = 0; i < _values.Length; i++)
            {
                if (_values[i].Equals(origin))
                    return _mirrors[i];
            }

            throw new CustomExceptions.CustomException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originValue"></param>
        /// <param name="mirror"></param>
        /// <returns></returns>
        public bool TryGetMirror(TValue originValue, out UMirror mirror)
        {
            mirror = default;
            for (var i = 0; i < _values.Length; i++)
            {
                if (!_values[i].Equals(originValue))
                    continue;
                
                mirror = _mirrors[i];
                return true;
            }
            Debug.Log("Unable to retrieve mirror from value");
            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pair"></param>
        public void AddPair(CustomPair<TValue, UMirror> pair)
        {
            _values = (TValue[])_values.Append(pair.Value);
            _mirrors = (UMirror[])_mirrors.Append(pair.Mirror);
        }
        
        #endregion
    }
}