using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Recurity.Swf.AVM1;
using Recurity.Swf.AVM1.Stack;
using Recurity.Swf.Flowgraph;
using Recurity.Swf.AVM1Modifier.CheckMachine;

namespace Recurity.Swf.AVM1Modifier.BuildingBlocks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractBuildingBlock
    {
        /// <summary>
        /// 
        /// </summary>
        private string _Identity;

        /// <summary>
        /// 
        /// </summary>
        protected List<string> _InlineSource;

        /// <summary>
        /// 
        /// </summary>
        private void GenerateIdentity()
        {
            if ( ( null != _Identity ) && ( _Identity.Length != 0 ) )
            {
                throw new InvalidOperationException( "Identity has been set already" );
            }

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] random = new byte[ 16 ];

            rng.GetBytes( random );

            StringBuilder sb = new StringBuilder();
            sb.Append( "BLZ" );    

            for ( int i = 0; i < random.Length; i++ )
            {
                sb.AppendFormat( "{0:X02}", random[ i ] );
            }

            _Identity = sb.ToString();
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected string Identity
        {
            get
            {
                if ( null == _Identity )
                {
                    GenerateIdentity();
                }
                return _Identity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> InlineSource 
        {
            get
            {
                return _InlineSource;
            }
        }        
        
        // link to the CheckMachine
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <param name="mStack"></param>
        /// <returns></returns>
        public abstract bool Execute( AVM1Code code, int instructionIndex, MachineStack mStack );
    }
}
