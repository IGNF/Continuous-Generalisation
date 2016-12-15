using System;
using System.Collections.Generic;
using System.Text;

namespace MorphingClass.CUtility
{
    public class CParameterThreshold
    {


        private double _dblAngleBound;
        private double _dblAreaBound;
        private double _dblDAreaBound;
        private double _dblUAreaBound;
        private double _dblLengthBound;
        private double _dblDLengthBound;
        private double _dblULengthBound;
        private double _dblRatioBound;
        

        private double _dblDCutBound;
        private double _dblUCutBound;


        private double _dblDLimit;
        private double _dblULimit;
        private double _dblSADLimit;
        private double _dblSAULimit;

        private double _dblSDLimit;
        private double _dblSULimit;
        private double _dblDepthSumRatio;
        private double _dblLengthSumRatio;

        private double _dblOverlapRatio;
        private double _dblBuffer;


        private double _dblFrLength;
        private double _dblToLength;


        /// <summary>���ԣ��Ƕ���ֵ</summary>
        public double dblAngleBound
        {
            get { return _dblAngleBound; }
            set { _dblAngleBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblAreaBound
        {
            get { return _dblAreaBound; }
            set { _dblAreaBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblDAreaBound
        {
            get { return _dblDAreaBound; }
            set { _dblDAreaBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblUAreaBound
        {
            get { return _dblUAreaBound; }
            set { _dblUAreaBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblLengthBound
        {
            get { return _dblLengthBound; }
            set { _dblLengthBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblDLengthBound
        {
            get { return _dblDLengthBound; }
            set { _dblDLengthBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblULengthBound
        {
            get { return _dblULengthBound; }
            set { _dblULengthBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblRatioBound
        {
            get { return _dblRatioBound; }
            set { _dblRatioBound = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblDLimit
        {
            get { return _dblDLimit; }
            set { _dblDLimit = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblULimit
        {
            get { return _dblULimit; }
            set { _dblULimit = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblSADLimit
        {
            get { return _dblSADLimit; }
            set { _dblSADLimit = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblSAULimit
        {
            get { return _dblSAULimit; }
            set { _dblSAULimit = value; }
        }


        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblSDLimit
        {
            get { return _dblSDLimit; }
            set { _dblSDLimit = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblSULimit
        {
            get { return _dblSULimit; }
            set { _dblSULimit = value; }
        }

                /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblLengthSumRatio
        {
            get { return _dblLengthSumRatio; }
            set { _dblLengthSumRatio = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblDepthSumRatio
        {
            get { return _dblDepthSumRatio; }
            set { _dblDepthSumRatio = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblOverlapRatio
        {
            get { return _dblOverlapRatio; }
            set { _dblOverlapRatio = value; }
        }

        /// <summary>���ԣ�Y����ƫ�ع�ϵ��</summary>
        public double dblBuffer
        {
            get { return _dblBuffer; }
            set { _dblBuffer = value; }
        }

        /// <summary>���ԣ����и��</summary>
        public double dblDCutBound
        {
            get { return _dblDCutBound; }
            set { _dblDCutBound = value; }
        }

        /// <summary>���ԣ����и��</summary>
        public double dblUCutBound
        {
            get { return _dblUCutBound; }
            set { _dblUCutBound = value; }
        }

        /// <summary>���ԣ����������״Ҫ�س���</summary>
        public double dblFrLength
        {
            get { return _dblFrLength; }
            set { _dblFrLength = value; }
        }

        /// <summary>���ԣ�С��������״Ҫ�س���</summary>
        public double dblToLength
        {
            get { return _dblToLength; }
            set { _dblToLength = value; }
        }



        //double dblUAreaBoundRatio = dblDepthSumRatio + dblAreaBound;
        //double dblDAreaBoundRatio = dblDepthSumRatio - dblAreaBound;
        //double dblDLimit = 1 / dblULimit;


        //double dblLeftRatio = CToBend.CLeftBend.dblBendDepthSum / CFromBend.CLeftBend.dblBendDepthSum;
        //double dblRightRatio = CToBend.CRightBend.dblBendDepthSum / CFromBend.CRightBend.dblBendDepthSum;
    }
}
