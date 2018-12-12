using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Display;

namespace MorphingClass.CUtility
{
    /// <summary>
    /// ��ʼ�������࣬�̳��ڲ�����
    /// </summary>
    /// <remarks></remarks>
    public class CParameterInitialize
    {
        private IFeatureLayer _pBSFLayer;         //�������Ҫ��ͼ��(LargerScaleFeatureLayer)
        private IFeatureLayer _pFeatureLayer;     //Ҫ��ͼ��
        private IFeatureLayer _pSSFLayer;         //С������Ҫ��ͼ��(SmallerScaleFeatureLayer)
        private IFeatureLayer _pSgFLayer;         //Layer of single features        
        private List<IFeatureLayer> _pFLayerLt;
        //private List<List<CPolyline>> _CPlLtLt;

        
        public Form frmContinuousGeneralizer { get; set; }
        //public ESRI.ArcGIS.Controls.ax

        public AxMapControl pAxMapControl { get; set; }
        private IMap _pMap;                       //��ͼ�ĵ�
        private IMap _m_mapPolyline;              //��ͼ�㼯
        //private IMap _m_mapPoints;                //��ͼ�㼯
        private IMap _m_mapAll;                   //����ͼ�㼯
        private IMap _m_mapFeature;                //��ͼ�㼯

        //private ISpatialReference _pSpatialReference; //����ο�ϵ
        private IWorkspace _pWorkspace;            //������

        private IMapControl4 _m_mapControl;       //��ͼ�ؼ�
        //private ITOCControl2 _m_tocControl;       //ͼ��ؼ�

        public CheckBox chkCoincidentPoints { get; set; }
        public CheckBox chkSmallest { get; set; }
        public CheckBox chkTesting { get; set; }
        //private CheckedListBox _clstFields;       //��ֵ�ֶθ�ѡ��

        private ComboBox _cboLargerScaleLayer;       //�������ͼ��
        private ComboBox _cboSmallerScaleLayer;     //С������ͼ��
        private ComboBox _cboSingleLayer;
        private List<ComboBox> _cboLayerLt;
        //private ComboBox _cboElevation;           //�߳��ֶ�
        //private ComboBox _cboField;               //��ֵ�ֶ�
        private ComboBox _cboLayer;               //��ֵͼ��        
        //private ComboBox _cboTimeType;            //����ʱ��ķ�ʽ
        private ComboBox _cboMorphingMethod;            //
        public ComboBox cboBuffer { get; set; }
        public ComboBox cboStandardVector { get; set; }
        public ComboBox cboEvaluationMethod { get; set; }
        public ComboBox cboIntMaxBackKforJ { get; set; }
        public ComboBox cboTransform { get; set; }
        public ComboBox cboShapeConstraint { get; set; }
        public Dictionary<int, int> intILPFailedIDDt { get; set; }

        //private double _dblBendDelRatio;          //����ɾ����
        //private double _dblDistance;              //����������
        //private double _dblError;                 //Bezier����̽����������ֵ
        //private double _dblLengthBound;           //��Ӧ���ж���ֵ    
        //private double _dblAngleBound;           //��Ӧ���ж���ֵ 
        //private double _dblPower;                 //������ָ��
        private double _dblOverlapRatio;          //�ص����ʲ���
        //private double _dblWeight;                //Ȩ��
        private double _dblLargerScale;          //һ������£��洢�˽ϴ�����ߵķ�ĸ
        private double _dblSmallerScale;         //һ������£��洢�˽�С�����ߵķ�ĸ
        private double _dblTargetScale;          //һ������£��洢��Ŀ������ߵķ�ĸ
        private double _dblLevelExponent;         //���ָ����
        private double _dblOrderExponent;         //�ȼ�ָ����


        //private int _intMulti;                     //sometimes we need to look for the smallest sufficient look-back parameter, so we need to test a series (_intMulti) look-back parameters
        private int _intMaxBackK;                 //look-back parameter K
        //private int _intOrder;                    //����ʽ����

        //private TextBox _txtBendDelRatio;         //����ɾ����
        private TextBox _txtMulti;                //sometimes we need to look for the smallest sufficient look-back parameter, so we need to test a series (_intMulti) look-back parameters
        private TextBox _txtIncrease;
        private TextBox _txtMaxBackK;             //look-back parameter K
        private TextBox _txtError;                //Bezier����̽����������ֵ
        private TextBox _txtLengthBound;          //��Ӧ���ж���ֵ
        private TextBox _txtAngleBound;          //��Ӧ���ж���ֵ
        //private TextBox _txtOrder;                //����ʽ����
        private TextBox _txtOverlapRatio;         //�ص����ʲ���
        //private TextBox _txtPath;                 //����դ��Ĵ洢·��
        //private TextBox _txtPower;                //������ָ��
        //private TextBox _txtWeight;               //Ȩ��
        private TextBox _txtLargerScale;          //һ������£�����ϴ�����ߵķ�ĸ
        private TextBox _txtSmallerScale;         //һ������£������С�����ߵķ�ĸ
        private TextBox _txtLevelExponent;        //���ָ����
        private TextBox _txtOrderExponent;        //�ȼ�ָ����
        private TextBox _txtInterpolationNum;     //�ڲ���״Ҫ������
        private TextBox _txtVtPV;                 //ƽ���е�һ��ֵ
        private TextBox _txtT;                    //���γ̶Ȳ���t
        private TextBox _txtIterationNum;         //��������
        public TextBox txtEvaluation { get; set; }
        public TextBox txtAttributeOfKnown { get; set; }

        private StatusStrip _ststMain;            //��̬��ʾ��
        private ToolStripStatusLabel _tsslTime;   //��ʱ��̬��ʾ����
        private ToolStripStatusLabel _tsslMessage;   //״̬��Ϣ��̬��ʾ����
        private ToolStripProgressBar _tspbMain;   //�����������

        //private string _strElevation;             //�߳��ֶ�
        //private string _strFieldName;             //�ֶ�����
        //private string _strGeneralizerMethod;        //Generalizer����
        //private string _strNewFileName;           //�½�ͼ�������

        private string _strSavePath;              //�洢·����
        private string _strSaveFolder;              //�洢·����
        //private string _strTimeType;              //����ʱ��ķ�ʽ(����ƽ��������ָ������ʱ�����)        

        /// <summary>The whole path to strSaveFolder, already including "\\" at the end, strSavePath == strPath + strLastName + "\\"</summary>
        public string strSavePathBackSlash { get; set; }
        public string strAreaAggregation { get; set; }

        /// <summary>The whole path to Data Folder, already including "\\" at the end</summary>
        public string strMxdPath { get; set; }
        public string strMxdPathBackSlash { get; set; }

        /// <summary>The whole path to strSaveFolder, without "\\" at the end, strSavePath == strPath + strLastName</summary>
        public string strSavePath { get; set; }

        /// <summary>The name of the folder used to save results, not a path</summary>
        public string strSaveFolderName { get; set; }

        /// <summary>choices of time and scale</summary>
        public string strTS { get; set; }

        /// <summary>���ԣ�������</summary>
        public IWorkspace pWorkspace { get; set; }
        public IWorkspace pFileGdbWorkspace { get; set; }

        //private List<string> _strFieldLt;         //�ֶ��б�����ͼ��ʱ����ӵ������ֶ�
        //private List<string> _strclstFieldsLt;    //�μ�ʱ���ֵ�ֶ���

        private Form _frmCurrentForm;

        /// <summary>���ԣ��������Ҫ��ͼ��(LargerScaleFeatureLayer)</summary>
        public IFeatureLayer pBSFLayer
        {
            get { return _pBSFLayer; }
            set { _pBSFLayer = value; }
        }

        /// <summary>���ԣ�Ҫ��ͼ��</summary>
        public IFeatureLayer pFeatureLayer
        {
            get { return _pFeatureLayer; }
            set { _pFeatureLayer = value; }
        }

        /// <summary>���ԣ�С������Ҫ��ͼ��(SmallerScaleFeatureLayer)</summary>
        public IFeatureLayer pSSFLayer
        {
            get { return _pSSFLayer; }
            set { _pSSFLayer = value; }
        }


        /// <summary>���ԣ�Layer of single features</summary>
        public IFeatureLayer pSgFLayer
        {
            get { return _pSgFLayer; }
            set { _pSgFLayer = value; }
        }

        public List<IFeatureLayer> pFLayerLt
        {
            get { return _pFLayerLt; }
            set { _pFLayerLt = value; }
        }


        //public AxMapControl pAxMapControl
        //{
        //    get { return _pAxMapControl; }
        //    set { _pAxMapControl = value; }
        //}


        ///// <summary>���ԣ���ͼ�ĵ�</summary>
        //public IMap pMap
        //{
        //    get { return _pMap; }
        //    set { _pMap = value; }
        //}

        /// <summary>���ԣ���ͼ�㼯</summary>
        public IMap m_mapPolyline
        {
            get { return _m_mapPolyline; }
            set { _m_mapPolyline = value; }
        }

        //public IMap m_mapPoints
        //{
        //    get { return _m_mapPoints; }
        //    set { _m_mapPoints = value; }
        //}

        public IMap m_mapFeature
        {
            get { return _m_mapFeature; }
            set { _m_mapFeature = value; }
        }

        ///// <summary>���ԣ�����ͼ�㼯</summary>
        //public IMap m_mapAll
        //{
        //    get { return _m_mapAll; }
        //    set { _m_mapAll = value; }
        //}

        /// <summary>���ԣ���ͼ�ؼ�</summary>
        public IMapControl4 m_mapControl
        {
            get { return _m_mapControl; }
            set { _m_mapControl = value; }
        }

        ///// <summary>���ԣ�ͼ��ؼ�</summary>
        //public ITOCControl2 m_tocControl
        //{
        //    get { return _m_tocControl; }
        //    set { _m_tocControl = value; }
        //}

        ///// <summary>���ԣ�ͼ��ؼ�</summary>
        //public ISpatialReference pSpatialReference
        //{
        //    get { return _pSpatialReference; }
        //    set { _pSpatialReference = value; }
        //}



        ///// <summary>���ԣ���ֵ�ֶθ�ѡ��</summary>
        //public CheckedListBox clstFields
        //{
        //    get { return _clstFields; }
        //    set { _clstFields = value; }
        //}

        /// <summary>���ԣ��������ͼ��</summary>
        public ComboBox cboLargerScaleLayer
        {
            get { return _cboLargerScaleLayer; }
            set { _cboLargerScaleLayer = value; }
        }

        ///// <summary>���ԣ��߳��ֶ�</summary>
        //public ComboBox cboElevation
        //{
        //    get { return _cboElevation; }
        //    set { _cboElevation = value; }
        //}

        ///// <summary>���ԣ���ֵ�ֶ�</summary>
        //public ComboBox cboField
        //{
        //    get { return _cboField; }
        //    set { _cboField = value; }
        //}

        /// <summary>���ԣ���ֵͼ��</summary>
        public ComboBox cboLayer
        {
            get { return _cboLayer; }
            set { _cboLayer = value; }
        }

        /// <summary>���ԣ�С������ͼ��</summary>
        public ComboBox cboSmallerScaleLayer
        {
            get { return _cboSmallerScaleLayer; }
            set { _cboSmallerScaleLayer = value; }
        }

        /// <summary>���ԣ�</summary>
        public ComboBox cboSingleLayer
        {
            get { return _cboSingleLayer; }
            set { _cboSingleLayer = value; }
        }

        public List<ComboBox> cboLayerLt
        {
            get { return _cboLayerLt; }
            set { _cboLayerLt = value; }
        }

        ///// <summary>���ԣ�����ʱ��ķ�ʽ</summary>
        //public ComboBox cboTimeType
        //{
        //    get { return _cboTimeType; }
        //    set { _cboTimeType = value; }
        //}

        public ComboBox cboMorphingMethod
        {
            get { return _cboMorphingMethod; }
            set { _cboMorphingMethod = value; }
        }


        /// <summary>���ԣ�look-back parameter K</summary>
        public int intMaxBackK
        {
            get { return _intMaxBackK; }
            set { _intMaxBackK = value; }
        }

        //        /// <summary>���ԣ�look-back parameter K</summary>
        //public int intMulti
        //{
        //    get { return _intMulti; }
        //    set { _intMulti = value; }
        //}


        ///// <summary>���ԣ�����ɾ����</summary>
        //public double dblBendDelRatio
        //{
        //    get { return _dblBendDelRatio; }
        //    set { _dblBendDelRatio = value; }
        //}

        ///// <summary>���ԣ���Ӧ���ж���ֵ</summary>
        //public double dblLengthBound
        //{
        //    get { return _dblLengthBound; }
        //    set { _dblLengthBound = value; }
        //}

        //public double dblAngleBound
        //{
        //    get { return _dblAngleBound; }
        //    set { _dblAngleBound = value; }
        //}

        ///// <summary>���ԣ�Bezier����̽����������ֵ</summary>
        //public double dblError
        //{
        //    get { return _dblError; }
        //    set { _dblError = value; }
        //}

        ///// <summary>���ԣ�����������</summary>
        //public double dblDistance
        //{
        //    get { return _dblDistance; }
        //    set { _dblDistance = value; }
        //}

        ///// <summary>���ԣ�������ָ��</summary>
        //public double dblPower
        //{
        //    get { return _dblPower; }
        //    set { _dblPower = value; }
        //}

        /// <summary>���ԣ��ص����ʲ���</summary>
        public double dblOverlapRatio
        {
            get { return _dblOverlapRatio; }
            set { _dblOverlapRatio = value; }
        }

        /// <summary>���ԣ����ָ����</summary>
        public double dblLevelExponent
        {
            get { return _dblLevelExponent; }
            set { _dblLevelExponent = value; }
        }

        /// <summary>���ԣ��ȼ�ָ����</summary>
        public double dblOrderExponent
        {
            get { return _dblOrderExponent; }
            set { _dblOrderExponent = value; }
        }

        /// <summary>���ԣ�һ������£��洢�˽ϴ�����ߵķ�ĸ</summary>
        public double dblLargerScale
        {
            get { return _dblLargerScale; }
            set { _dblLargerScale = value; }
        }

        /// <summary>���ԣ�һ������£��洢�˽�С�����ߵķ�ĸ</summary>
        public double dblSmallerScale
        {
            get { return _dblSmallerScale; }
            set { _dblSmallerScale = value; }
        }

        /// <summary>���ԣ�һ������£��洢��Ŀ������ߵķ�ĸ</summary>
        public double dblTargetScale
        {
            get { return _dblTargetScale; }
            set { _dblTargetScale = value; }
        }

        ///// <summary>���ԣ�����ʽ����</summary>
        //public int intOrder
        //{
        //    get { return _intOrder; }
        //    set { _intOrder = value; }
        //}

        ///// <summary>���ԣ�����ɾ����</summary>
        //public TextBox txtBendDelRatio
        //{
        //    get { return _txtBendDelRatio; }
        //    set { _txtBendDelRatio = value; }
        //}

        /// <summary>���ԣ�look-back parameter K</summary>
        public TextBox txtMaxBackK
        {
            get { return _txtMaxBackK; }
            set { _txtMaxBackK = value; }
        }

        /// <summary>���ԣ�look-back parameter K</summary>
        public TextBox txtMulti
        {
            get { return _txtMulti; }
            set { _txtMulti = value; }
        }

        public TextBox txtIncrease
        {
            get { return _txtIncrease; }
            set { _txtIncrease = value; }
        }


        /// <summary>���ԣ�Bezier����̽����������ֵ</summary>
        public TextBox txtError
        {
            get { return _txtError; }
            set { _txtError = value; }
        }

        /// <summary>���ԣ���Ӧ���ж���ֵ</summary>
        public TextBox txtLengthBound
        {
            get { return _txtLengthBound; }
            set { _txtLengthBound = value; }
        }

        public TextBox txtAngleBound
        {
            get { return _txtAngleBound; }
            set { _txtAngleBound = value; }
        }

        ///// <summary>���ԣ�����ʽ����</summary>
        //public TextBox txtOrder
        //{
        //    get { return _txtOrder; }
        //    set { _txtOrder = value; }
        //}

        /// <summary>���ԣ��ص����ʲ���</summary>
        public TextBox txtOverlapRatio
        {
            get { return _txtOverlapRatio; }
            set { _txtOverlapRatio = value; }
        }

        ///// <summary>���ԣ��洢·��</summary>
        //public TextBox txtPath
        //{
        //    get { return _txtPath; }
        //    set { _txtPath = value; }
        //}

        ///// <summary>���ԣ�������ָ��</summary>
        //public TextBox txtPower
        //{
        //    get { return _txtPower; }
        //    set { _txtPower = value; }
        //}

        ///// <summary>���ԣ�Ȩ��</summary>
        //public TextBox txtWeight
        //{
        //    get { return _txtWeight; }
        //    set { _txtWeight = value; }
        //}

        /// <summary>���ԣ����ָ����</summary>
        public TextBox txtLevelExponent
        {
            get { return _txtLevelExponent; }
            set { _txtLevelExponent = value; }
        }

        /// <summary>���ԣ��ȼ�ָ����</summary>
        public TextBox txtOrderExponent
        {
            get { return _txtOrderExponent; }
            set { _txtOrderExponent = value; }
        }

        /// <summary>���ԣ�һ������£�����ϴ�����ߵķ�ĸ</summary>
        public TextBox txtLargerScale
        {
            get { return _txtLargerScale; }
            set { _txtLargerScale = value; }
        }

        /// <summary>���ԣ�һ������£������С�����ߵķ�ĸ</summary>
        public TextBox txtSmallerScale
        {
            get { return _txtSmallerScale; }
            set { _txtSmallerScale = value; }
        }

        /// <summary>���ԣ��ڲ���״Ҫ������</summary>
        public TextBox txtInterpolationNum
        {
            get { return _txtInterpolationNum; }
            set { _txtInterpolationNum = value; }
        }

        /// <summary>���ԣ��ڲ���״Ҫ������</summary>
        public TextBox txtVtPV
        {
            get { return _txtVtPV; }
            set { _txtVtPV = value; }
        }

        /// <summary>���ԣ����γ̶Ȳ���</summary>
        public TextBox txtT
        {
            get { return _txtT; }
            set { _txtT = value; }
        }

        /// <summary>���ԣ���������</summary>
        public TextBox txtIterationNum
        {
            get { return _txtIterationNum; }
            set { _txtIterationNum = value; }
        }

        /// <summary>���ԣ���̬��ʾ��</summary>
        public StatusStrip ststMain
        {
            get { return _ststMain; }
            set { _ststMain = value; }
        }

        /// <summary>���ԣ���ʱ��̬��ʾ����</summary>
        public ToolStripStatusLabel tsslTime
        {
            get { return _tsslTime; }
            set { _tsslTime = value; }
        }

        /// <summary>���ԣ�״̬��Ϣ��̬��ʾ����</summary>
        public ToolStripStatusLabel tsslMessage
        {
            get { return _tsslMessage; }
            set { _tsslMessage = value; }
        }

        /// <summary>���ԣ������������</summary>
        public ToolStripProgressBar tspbMain
        {
            get { return _tspbMain; }
            set { _tspbMain = value; }
        }

        ///// <summary>���ԣ��߳��ֶ�</summary>
        //public string strElevation
        //{
        //    get { return _strElevation; }
        //    set { _strElevation = value; }
        //}

        ///// <summary>���ԣ��ֶ�����</summary>
        //public string strFieldName
        //{
        //    get { return _strFieldName; }
        //    set { _strFieldName = value; }
        //}


        ///// <summary>���ԣ�Generalizer����</summary>
        //public string strGeneralizerMethod
        //{
        //    get { return _strGeneralizerMethod; }
        //    set { _strGeneralizerMethod = value; }
        //}

        ///// <summary>���ԣ��½�ͼ�������</summary>
        //public string strNewFileName
        //{
        //    get { return _strNewFileName; }
        //    set { _strNewFileName = value; }
        //}







        ///// <summary>���ԣ�����ʱ��ķ�ʽ(����ƽ��������ָ������ʱ�����)</summary>
        //public string strTimeType
        //{
        //    get { return _strTimeType; }
        //    set { _strTimeType = value; }
        //}


        ///// <summary>���ԣ��ֶ��б�����ͼ��ʱ����ӵ������ֶ�</summary>
        //public List<string> strFieldLt
        //{
        //    get { return _strFieldLt; }
        //    set { _strFieldLt = value; }
        //}

        ///// <summary>���ԣ��ֶ��б�����ͼ��ʱ����ӵ������ֶ�</summary>
        //public List<string> strclstFieldsLt
        //{
        //    get { return _strclstFieldsLt; }
        //    set { _strclstFieldsLt = value; }
        //}


        public Form frmCurrentForm
        {
            get { return _frmCurrentForm; }
            set { _frmCurrentForm = value; }
        }






    }
}
