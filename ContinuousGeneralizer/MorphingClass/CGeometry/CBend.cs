using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;

using MorphingClass.CUtility;

namespace MorphingClass.CGeometry
{
    public class CBend: PolylineClass
    {
        /// <summary>
        /// Ϊ�˽�ʡʱ�䣬��ʼ������ʱ�����������߶Σ�����������ԭ�߶�˳��洢���ڵ�
        /// Ҫʹ�������߶��пռ��������ִ�к���FormBend�����������߶�֮�󣬷���ʹ��
        /// </summary>
        private List<CPoint> _CptLt=new List<CPoint> ();
        private List<CBend> _pBendLt;             //�����������б������غ�������
        private List<int> _intTIDLt=new List<int> ();
        private List<CTriangle> _CTriangleLt = new List<CTriangle>();  //���ڱ�������������
        private SortedList<double, CBend> _pCorrespondBendLt = new SortedList<double, CBend>(new CCompareDbl());  //��Ӧ�����б�
        private int _intPathCount;

        private int _ID;
        private int _intPathTriCount;   //·��TID���������������������ײ�������������֮�ͣ�Խ�߲�������α��ظ�����Ĵ���Խ�ࣩ
        private double _dblBendDepthAverage;  //��������ƽ�����
        private double _dblBendDepthMax;
        private double _dblBendDepthSum;
        private double _dblBendDepthSumHelp;
        private double _dblStartRL;        //��������ԭ��״Ҫ���ϵ������ʼλ��(Start Ratio Location)
        private double _dblEndRL;          //��������ԭ��״Ҫ���ϵ�����յ�λ��(End Ratio Location)

        private string _strSide = "Undefined";         //�����������ߵ����"Left"�����ұ�"Right"����ʼֵΪ"Undefined"

        private CBend _CParentBend;
        private CBend _CLeftBend;
        private CBend _CRightBend;

        private ILine _pBaseLine;     //��������(�����ɵ��С�ĵ�ָ���Ŵ�ĵ�)
        private CPolyline _pBaseLineDeep; //���������������������(�����ɵ��С�ĵ�ָ���Ŵ�ĵ�)

        private bool _isMatch;

        /// <summary>�����������б������غ�������</summary>
        public List<CBend> pBendLt
        {
            get { return _pBendLt; }
            set { _pBendLt = value; }
        }

        /// <summary>����</summary>
        public List<CPoint> CptLt
        {            
            get { return _CptLt; }
            set { _CptLt = value; }
        }

        /// <summary>���֧����</summary>
        public List<int> intTIDLt
        {
            get { return _intTIDLt; }
            set { _intTIDLt = value; }
        }

        /// <summary>���ڱ�������������</summary>
        public List<CTriangle> CTriangleLt
        {
            get { return _CTriangleLt; }
            set { _CTriangleLt = value; }
        }

        /// <summary>��Ӧ�����б�</summary>
        public SortedList<double, CBend> pCorrespondBendLt
        {
            get { return _pCorrespondBendLt; }
            set { _pCorrespondBendLt = value; }
        }

        /// <summary>�������</summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>��֧�������Ͳ������������</summary>
        public int intPathCount
        {
            get { return _intPathCount; }
            set { _intPathCount = value; }
        }

        /// <summary>·��TID���������������������ײ�������������֮�ͣ�Խ�߲�������α��ظ�����Ĵ���Խ�ࣩ</summary>
        public int intPathTriCount
        {
            get { return _intPathTriCount; }
            set { _intPathTriCount = value; }
        }

        /// <summary>��������ƽ�����</summary>
        public double dblBendDepthAverage
        {
            get { return _dblBendDepthAverage; }
            set { _dblBendDepthAverage = value; }
        }

        /// <summary>��������������</summary>
        public double dblBendDepthMax
        {
            get { return _dblBendDepthMax; }
            set { _dblBendDepthMax = value; }
        }
        
        /// <summary>��������������</summary>
        public double dblBendDepthSum
        {
            get { return _dblBendDepthSum; }
            set { _dblBendDepthSum = value; }
        }


        /// <summary>��������������</summary>
        public double dblBendDepthSumHelp
        {
            get { return _dblBendDepthSumHelp; }
            set { _dblBendDepthSumHelp = value; }
        }

        /// <summary>��������ԭ��״Ҫ���ϵ������ʼλ��(Start Ratio Location)</summary>
        public double dblStartRL
        {
            get { return _dblStartRL; }
            set { _dblStartRL = value; }
        }

        /// <summary>��������ԭ��״Ҫ���ϵ�����յ�λ��(End Ratio Location)</summary>
        public double dblEndRL
        {
            get { return _dblEndRL; }
            set { _dblEndRL = value; }
        }

        /// <summary>������λ����״Ҫ�ص���һ��</summary>
        public string strSide
        {
            get { return _strSide; }
            set { _strSide = value; }
        }

        /// <summary>��������</summary>
        public CBend CParentBend
        {
            get { return _CParentBend; }
            set { _CParentBend = value; }
        }

        /// <summary>���֧����</summary>
        public CBend CLeftBend
        {
            get { return _CLeftBend; }
            set { _CLeftBend = value; }
        }

        /// <summary>�ҷ�֧����</summary>
        public CBend CRightBend
        {
            get { return _CRightBend; }
            set { _CRightBend = value; }
        }

        /// <summary>��֧�������Ͳ������������</summary>
        public bool isMatch
        {
            get { return _isMatch; }
            set { _isMatch = value; }
        }

        /// <summary>��������</summary>
        public ILine pBaseLine
        {
            get { return _pBaseLine; }
            set { _pBaseLine = value; }
        }

        /// <summary>���������������������</summary>
        public CPolyline pBaseLineDeep
        {
            get { return _pBaseLineDeep; }
            set { _pBaseLineDeep = value; }
        }


        //public CBend()
        //{


        //}

        public CBend(List<CPoint> cptlt)
        {
            _CptLt = cptlt;
            FormBend(cptlt);
        }

        public CBend(List <CPoint > cptlt,string strSide)
        {
            _CptLt = cptlt;
            _strSide = strSide;
            FormBend(cptlt);
        }

        

        /// <summary>
        /// ͨ�����������ڵ��ȡ��������������
        /// </summary>
        /// <param name="cpt1">���ȡ���������ϵ��յ�֮һ</param>
        /// <param name="cpt2">���ȡ���������ϵ��յ�֮һ 2</param>
        public CBend GetSubBend(CPoint cpt1,CPoint cpt2, string strSide,double dblVerySmall)
        {
            if (cpt1.Equals2D(cpt2) == true)
            {
                MessageBox.Show("��������ͬ���޷���ȡ����");
                return null;
            }

            List<CPoint> ptlt = new List<CPoint>();
            int intBegin = 0;
            for (int i = 0; i < _CptLt.Count ; i++)
            {
                //��һ��������ͬ���ʱ��intBegin==1������������ȡ�����ĳ�ʼ�㣻�ڶ���������ͬ��ʱ��intBegin==2������������ȡ�������յ�
                if (_CptLt[i].Equals2D(cpt1, dblVerySmall) == true || _CptLt[i].Equals2D(cpt2, dblVerySmall) == true) intBegin = intBegin + 1;
                if (intBegin > 0) ptlt.Add(_CptLt[i]);
                if (intBegin == 2) break;
            }

            CBend CSubBend = new CBend(ptlt, strSide);
            return CSubBend;
        }


        /// <summary>
        /// ͨ�����������ڵ�ĵ�Ż�ȡ��������������
        /// </summary>
        /// <param name="ID1">���ȡ���������ϵ��յ�֮һ�ĵ��</param>
        /// <param name="ID1">���ȡ���������ϵ��յ�֮һ�ĵ�� 2</param>
        public CBend GetSubBend(int ID1, int ID2, string strSide)
        {
            if (ID1 == ID2)
            {
                MessageBox.Show("��������ͬ���޷���ȡ����");
                return null;
            }

            List<CPoint> ptlt = new List<CPoint>();
            int intBegin = 0;
            for (int i = 0; i < _CptLt.Count; i++)
            {
                //��һ��������ͬ���ʱ��intBegin==1������������ȡ�����ĳ�ʼ�㣻�ڶ���������ͬ��ʱ��intBegin==2������������ȡ�������յ�
                if (_CptLt[i].ID == ID1 || _CptLt[i].ID == ID2) intBegin = intBegin + 1;
                if (intBegin > 0) ptlt.Add(_CptLt[i]);
                if (intBegin == 2) break;
            }

            CBend CSubBend = new CBend(ptlt, strSide);
            return CSubBend;
        }













        /// <summary>
        /// ����ԭ�߶η�����������
        /// </summary>
        /// <remarks>����Ѿ����ɹ��ߣ���ֱ�ӷ���</remarks>
        public void FormBend(List <CPoint > cptlt)
        {
            object Missing = Type.Missing;
            IPointCollection4 pCol = new PolylineClass();
            for (int i = 0; i < cptlt.Count; i++)
            {
                pCol.AddPoint((IPoint)cptlt[i], ref Missing, ref Missing);
            }
            this.SetPointCollection(pCol);

            //���ɻ���
            _pBaseLine = new LineClass();
            _pBaseLine.PutCoords((IPoint)cptlt[0], (IPoint)cptlt[cptlt.Count - 1]);
            
        }





    }
}
