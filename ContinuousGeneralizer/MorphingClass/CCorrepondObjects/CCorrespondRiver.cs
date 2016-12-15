using System;
using System.Collections.Generic;
using System.Text;

using MorphingClass.CEntity;

namespace MorphingClass.CCorrepondObjects
{
    public class CCorrespondRiver
    {
        private CRiver _CFromRiver;           //������߱�����
        private CRiver _CToRiver;             //С�����߱�����
        private bool _blnCorr;                //�Ƿ��ж�Ӧ����
       

        public CCorrespondRiver()
        {

        }

        public CCorrespondRiver(CRiver pCFromRiver, CRiver pCToRiver)
        {
            _CFromRiver = pCFromRiver;
            _CToRiver = pCToRiver;
        }


        /// <summary>���ԣ�������߱�����</summary>
        public CRiver CFromRiver
        {
            get { return _CFromRiver; }
            set { _CFromRiver = value; }
        }

        /// <summary>���ԣ�С�����߱�����</summary>
        public CRiver CToRiver
        {
            get { return _CToRiver; }
            set { _CToRiver = value; }
        }

        /// <summary>���ԣ��Ƿ��ж�Ӧ����</summary>
        public bool blnCorr
        {
            get { return _blnCorr; }
            set { _blnCorr = value; }
        }


    }
}
