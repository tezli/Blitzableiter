using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1.Stack
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM1Stack
    {
        /// <summary>
        /// 
        /// </summary>
        private int _SP;

        /// <summary>
        /// 
        /// </summary>
        private List<AVM1DataElement> _InternalStack;

        /// <summary>
        /// 
        /// </summary>
        public AVM1Stack()
        {
            _SP = 0;
            _InternalStack = new List<AVM1DataElement>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int SP { get { return _SP; } set { _SP = value; } }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get { return _SP; } }
        
        /// <summary>
        /// Gets the item that is the specified number of elements below the stack top. 0 is the top.
        /// </summary>
        /// <param name="distanceFromStackTop">How far to look down (0=not)</param>
        /// <returns>Stack Item</returns>
        public AVM1DataElement this[ uint distanceFromStackTop ]
        {
            get
            {
                int realIndex = ( _InternalStack.Count - 1 ) - (int)distanceFromStackTop;
                if ( realIndex >= 0 )
                {
                    return _InternalStack[ realIndex ];
                }
                else
                {
                    AVM1DataElement e = new AVM1DataElement();
                    e.Value = null;
                    e.DataType = AVM1DataTypes.AVM_UNKNOWN;
                    return e;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void Push( AVM1DataElement element )
        {
            _SP++;
            if ( _SP >= 0 )
                _InternalStack.Add( element );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AVM1DataElement Pop()
        {
            _SP--;
            if ( _SP > 0 )
            {
                AVM1DataElement e = _InternalStack[ _InternalStack.Count - 1 ];
                _InternalStack.RemoveAt( _InternalStack.Count - 1 );
                return e;
            }
            else
            {
                AVM1DataElement e = new AVM1DataElement();
                e.Value = null;
                e.DataType = AVM1DataTypes.AVM_UNKNOWN;
                return e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _SP = 0;
            _InternalStack.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public void Fill( AVM1DataElement[] elements )
        {
            _InternalStack.Clear();
            for ( int i = 0; i < elements.Length; i++ )
            {
                _InternalStack.Add( elements[ i ] );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void CopyTo( AVM1Stack destination )
        {
            destination.Clear();
            destination.SP = _SP;
            destination.Fill( _InternalStack.ToArray() );
        }

    }
}
