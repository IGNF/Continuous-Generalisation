using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
//System.Runtime.InteropServices

namespace MorphingClass.CGeometry
{
    //[ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class CBendForest : SortedDictionary<int,  CBend>
    {
        private double _dblDepthAverage;
        private double _dblDepthMax;
        private List<CBend> _pBendLt;             //�����������б������غ�������
        private List<double> _dblDeepBendDepthLt;
        private int _intPathCount;
        

        /// <summary>�����ߵ�ƽ�����</summary>
        public double dblDepthAverage
        {
            get { return _dblDepthAverage; }
            set { _dblDepthAverage = value; }
        }

        /// <summary>�����ߵ�������</summary>
        public double dblDepthMax
        {
            get { return _dblDepthMax; }
            set { _dblDepthMax = value; }
        }

        /// <summary>�ײ�������ȼ�¼</summary>
        public List<double> dblDeepBendDepthLt
        {
            get { return _dblDeepBendDepthLt; }
            set { _dblDeepBendDepthLt = value; }
        }


        /// <summary>��֧�������Ͳ������������</summary>
        public int intPathCount
        {
            get { return _intPathCount; }
            set { _intPathCount = value; }
        }

        /// <summary>�����������б������غ�������</summary>
        public List<CBend> pBendLt
        {
            get { return _pBendLt; }
            set { _pBendLt = value; }
        }

        //����	1	���Ϳ⵼�������ڴ���MorphingClass.CGeometry.CBendForest, MorphingClass��ʱ�������档����: ���Ϳ⵼�����������ӷ�������������δ���Ϊ [ClassInterface(ClassInterfaceType.None)] �����͡��޷������������͵���ӿڡ��뿼���� [ClassInterface(ClassInterfaceType.None)] ��Ǹ����ͣ���ʹ�� ComDefaultInterface ������ COM ����ĳ����ʽ�ӿ���ΪĬ�Ͻӿڡ�	MorphingClass








    }
}
