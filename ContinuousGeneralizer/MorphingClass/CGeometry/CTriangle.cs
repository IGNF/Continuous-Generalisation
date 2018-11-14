using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ESRI.ArcGIS.Geometry;

namespace MorphingClass.CGeometry
{
    public class CTriangle : PolygonClass
    {
        /// <summary>
        /// Ϊ�˽�ʡʱ�䣬��ʼ�������ζ����ʱ�ǲ����������εıߺͶ���εģ���������˳ʱ�뷽��洢�����ε��������㡣
        /// Ҫʹ�������εıߣ�����Σ����пռ��������ִ�к���FormCEdge��FormPolygon�������ɱߣ�����Σ�֮�󣬷���ʹ��
        /// </summary>
        /// <remarks>Ϊ�˷���ʹ�ã��������ν�����ʱ��˳�㽨��һЩ��Ӧ��ϵ����_CEdgeLt[i]һ���Ǳ���������������_SETriangleLt[i]�Ĺ����ߣ�
        /// �ұ�_CEdgeLt[i]һ���ǵ�_CptLt[i]�ĶԱ�</remarks>

        private int _intTID;
        private int _intSETriangleNum;    // ��������������(SETriangle: Share Edge Triangle)
        private List<CPoint> _CptLt;      // �����εĶ�����(˳ʱ��)
        private List<CEdge> _CEdgeLt;     // �����εı���(˳ʱ��)
        private List<CTriangle> _SETriangleLt;

        private bool _blnBuildBend;       //��ʶ�Ƿ�������

        private bool _isBuildBendVisit;   //�Ƿ��ڽ��������Ĳ�νṹʱ�Ѿ����ʹ���
        private bool _isCrustTriangle;    //��������Σ��������������ζ����������
        private bool _isNeedSide;         // �Ƿ��ж������ڽ���ĳ��Լ����������
        private bool _isNeedSide2;        // �Ƿ��ж������ڽ���ĳ��Լ����������
        private bool _isNeedSide3;        // �Ƿ��ж������ڽ���ĳ��Լ����������
        private bool _isSuperTriangle;    // �Ƿ�������
        private bool _isSideJudge;        // �Ƿ��ж������ڽ���ĳ��Լ����������
        private bool _isSideJudge2;       // �Ƿ��ж������ڽ���ĳ��Լ����������
        private bool _isRightSide;        // �Ƿ����ұߣ����ڽ���ĳ��Լ����������
        
        private bool _isTriTypeJudge;     // �Ƿ�ȷ��Ϊĳ��������



        private CPoint _CentroidCpt;

        private string _strTriType;       //���������ͣ���Ϊ��I�ࡱ��II�ࡱ��III�ࡱ��IV�ࡱ
        private string _strSide = "Undefined";            //���ߵ����"Left"�����ұ�"Right"����ʼֵΪ"Undefined"


        /// <summary>
        /// ��ʶ�Ƿ�������
        /// </summary>
        public bool blnBuildBend
        {
            get { return _blnBuildBend; }
            set { _blnBuildBend = value; }
        }


        /// <summary>
        /// �Ƿ��ڽ��������Ĳ�νṹʱ�Ѿ����ʹ���
        /// </summary>
        public bool isBuildBendVisit
        {
            get { return _isBuildBendVisit; }
            set { _isBuildBendVisit = value; }
        }


        /// <summary>
        /// ��������Σ��������������ζ����������
        /// </summary>
        public bool isCrustTriangle
        {
            get { return _isCrustTriangle; }
            set { _isCrustTriangle = value; }
        }

        /// <summary>
        /// �Ƿ��ж������ڽ���ĳ��Լ����������
        /// </summary>
        public bool isNeedSide
        {
            get { return _isNeedSide; }
            set { _isNeedSide = value; }
        }

        /// <summary>
        /// �Ƿ��ж������ڽ���ĳ��Լ����������
        /// </summary>
        public bool isNeedSide2
        {
            get { return _isNeedSide2; }
            set { _isNeedSide2 = value; }
        }

        /// <summary>
        /// �Ƿ��ж������ڽ���ĳ��Լ����������
        /// </summary>
        public bool isNeedSide3
        {
            get { return _isNeedSide3; }
            set { _isNeedSide3 = value; }
        }

        /// <summary>
        /// �Ƿ��ж������ڽ���ĳ��Լ����������
        /// </summary>
        public bool isSideJudge
        {
            get { return _isSideJudge; }
            set { _isSideJudge = value; }
        }

        /// <summary>
        /// �Ƿ��ж������ڽ���ĳ��Լ����������
        /// </summary>
        public bool isSideJudge2
        {
            get { return _isSideJudge2; }
            set { _isSideJudge2 = value; }
        }


        /// <summary>
        /// �Ƿ����ұߣ����ڽ���ĳ��Լ����������
        /// </summary>
        public bool isRightSide
        {
            get { return _isRightSide; }
            set { _isRightSide = value; }
        }

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        public bool isSuperTriangle
        {
            get { return _isSuperTriangle; }
            set { _isSuperTriangle = value; }
        }

        /// <summary>
        /// �����ε����
        /// </summary>
        public int TID
        {
            get { return _intTID; }
            set { _intTID = value; }
        }

        /// <summary>
        /// ���������θ���
        /// </summary>
        public int SETriangleNum
        {
            get { return _intSETriangleNum; }
            set { _intSETriangleNum = value; }
        }


        /// <summary>
        /// �����εĶ�����(˳ʱ��)
        /// </summary>
        public List<CPoint> CptLt
        {
            get { return _CptLt; }
        }

        /// <summary>
        /// �����εı���(˳ʱ��)
        /// </summary>
        public List<CEdge> CEdgeLt
        {
            get { return _CEdgeLt; }
        }

        /// <summary>
        /// ����������
        /// </summary>
        public List<CTriangle> SETriangleLt
        {
            get { return _SETriangleLt; }
            set { _SETriangleLt = value; }
        }

        /// <summary>
        /// �Ƿ�ȷ��Ϊĳ��������
        /// </summary>
        public bool isTriTypeJudge
        {
            get { return _isTriTypeJudge; }
            set { _isTriTypeJudge = value; }
        }


        /// <summary>
        /// ���������ĵ�
        /// </summary>
        public CPoint CentroidCpt
        {
            get
            {
                _CentroidCpt = new CPoint();
                for (int i = 0; i < _CptLt.Count; i++)
                {
                    _CentroidCpt.X = _CentroidCpt.X + _CptLt[i].X;
                    _CentroidCpt.Y = _CentroidCpt.Y + _CptLt[i].Y;

                }
                _CentroidCpt.X = _CentroidCpt.X / 3;
                _CentroidCpt.Y = _CentroidCpt.Y / 3;
                return _CentroidCpt;
            }
        }

        /// <summary>
        /// ���������ͣ�"I"��"II"��"III"��"VI"
        /// </summary>
        public string strTriType
        {
            get { return _strTriType; }
            set { _strTriType = value; }
        }


        /// <summary>���ߵ����"Left"�����ұ�"Right"</summary>
        public string strSide
        {
            get { return _strSide; }
            set { _strSide = value; }
        }

        /// <summary>
        /// Initializes a new instance of a triangle
        /// </summary>
        public CTriangle(int intTID)
        {
            _intTID = intTID;
        }

        /// <summary>
        /// Initializes a new instance of a triangle
        /// </summary>
        /// <param name="point1">Vertex 1</param>
        /// <param name="point2">Vertex 2</param>
        /// <param name="point3">Vertex 3</param>
        public CTriangle(CPoint point1, CPoint point2, CPoint point3)
        {
            List<CPoint> cptlt = new List<CPoint>(3);
            cptlt.Add(point1);
            cptlt.Add(point2);
            cptlt.Add(point3);
            _CptLt = cptlt;

            _intTID = -1;
            FormCEdge(cptlt);
            FormPolygon(cptlt);
        }

        /// <summary>
        /// Initializes a new instance of a triangle
        /// </summary>
        /// <param name="point1">Vertex 1</param>
        /// <param name="point2">Vertex 2</param>
        /// <param name="point3">Vertex 3</param>
        public CTriangle(int intTID, CPoint point1, CPoint point2, CPoint point3)
        {

            List<CPoint> cptlt = new List<CPoint>(3);
            cptlt.Add(point1);
            cptlt.Add(point2);
            cptlt.Add(point3);
            _CptLt = cptlt;

            _intTID = intTID;
            FormCEdge(cptlt);
            FormPolygon(cptlt);
        }


        /// <summary>
        /// ���ɱ�(˳ʱ��)
        /// </summary>
        /// <remarks>����Ѿ����ɹ��ߣ���ֱ�ӷ���</remarks>
        private void FormCEdge(List<CPoint> cptlt)
        {
            CEdge Edge0 = new CEdge(_CptLt[1], _CptLt[2]);
            CEdge Edge1 = new CEdge(_CptLt[2], _CptLt[0]);
            CEdge Edge2 = new CEdge(_CptLt[0], _CptLt[1]);

            List<CEdge> cedgelt = new List<CEdge>(3);
            cedgelt.Add(Edge0);
            cedgelt.Add(Edge1);
            cedgelt.Add(Edge2);
            _CEdgeLt = cedgelt;
        }

        /// <summary>
        /// ���ɶ����(˳ʱ��)
        /// </summary>
        /// <remarks >����Ѿ����ɹ��ߣ���ֱ�ӷ���</remarks>
        private void FormPolygon(List<CPoint> cptlt)
        {
            //�����ռ��������������������ε���������
            IPointCollection4 pPntCtl = new PolygonClass();
            object Missing = Type.Missing;
            pPntCtl.AddPoint((IPoint)_CptLt[0], ref Missing, ref Missing);
            pPntCtl.AddPoint((IPoint)_CptLt[1], ref Missing, ref Missing);
            pPntCtl.AddPoint((IPoint)_CptLt[2], ref Missing, ref Missing);

            //�������еĵ��Զ���εķ�ʽ���
            this.SetPointCollection(pPntCtl);
            this.Close();
        }

    }
}
