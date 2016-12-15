using System;
using System.Collections.Generic;
using System.Text;

using MorphingClass.CEntity;

namespace MorphingClass.CCorrepondObjects
{
    public class CCorrAtBds
    {
        private CAtBd _pBSAtBd;           //������߱�����
        private CAtBd _pSSAtBd;             //С�����߱�����
        private bool _blnCorr;                //�Ƿ��ж�Ӧ����
       

        public CCorrAtBds()
        {

        }

        public CCorrAtBds(CAtBd fBSAtBd, CAtBd fSSAtBd)
        {
            _pBSAtBd = fBSAtBd;
            _pSSAtBd = fSSAtBd;
        }


        /// <summary>���ԣ�������߱�����</summary>
        public CAtBd pBSAtBd
        {
            get { return _pBSAtBd; }
            set { _pBSAtBd = value; }
        }

        /// <summary>���ԣ�С�����߱�����</summary>
        public CAtBd pSSAtBd
        {
            get { return _pSSAtBd; }
            set { _pSSAtBd = value; }
        }

        /// <summary>���ԣ��Ƿ��ж�Ӧ����</summary>
        public bool blnCorr
        {
            get { return _blnCorr; }
            set { _blnCorr = value; }
        }


    }
}
