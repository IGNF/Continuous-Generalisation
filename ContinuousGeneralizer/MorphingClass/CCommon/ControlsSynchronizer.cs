// Copyright 2006 ESRI
//
// All rights reserved under the copyright laws of the United States
// and applicable international laws, treaties, and conventions.
//
// You may freely redistribute and use this sample code, with or
// without modification, provided you include the original copyright
// notice and use restrictions.
//
// See use restrictions at /arcgis/developerkit/userestrictions.
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;

namespace MorphingClass.CCommon
{
    /// <summary>
    /// This class is used to synchronize a gven PageLayoutControl and a MapControl.
    /// When initialized, the user must pass the reference of these control to the class, bind
    /// the control together by calling 'BindControls' which in turn sets a joined Map referenced
    /// by both control; and set all the buddy controls joined between these two controls.
    /// When alternating between the MapControl and PageLayoutControl, you should activate the visible control
    /// and deactivate the other by calling ActivateXXX.
    /// This calss is limited to a situation where the controls are not simultaneously visible.
    /// </summary>
    public class ControlsSynchronizer
    {
        #region class members
        private IMapControl4 m_mapControl = null;
        private IPageLayoutControl2 m_pageLayoutControl = null;
        private ITool m_mapActiveTool = null;
        private ITool m_pageLayoutActiveTool = null;
        private bool m_IsMapCtrlactive = true;
        private ArrayList m_frameworkControls = null;
        #endregion
        #region constructor
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public ControlsSynchronizer()
        {
            //��ʼ��ArrayList
            m_frameworkControls = new ArrayList();
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="mapControl"></param>
        /// <param name="pageLayoutControl"></param>
        public ControlsSynchronizer(IMapControl4 mapControl, IPageLayoutControl2 pageLayoutControl)
            : this()
        {
            //Ϊ���Ա��ֵ
            m_mapControl = mapControl;
            m_pageLayoutControl = pageLayoutControl;
        }
        #endregion
        #region properties


        /// <summary>
        /// ȡ�û�����MapControl
        /// </summary>
        public IMapControl4 MapControl
        {
            get { return m_mapControl; }
            set { m_mapControl = value; }
        }

        /// <summary>
        /// ȡ�û�����PageLayoutControl
        /// </summary>
        public IPageLayoutControl2 PageLayoutControl
        {
            get { return m_pageLayoutControl; }
            set { m_pageLayoutControl = value; }
        }

        /// <summary>
        /// ȡ�õ�ǰActiveView������
        /// </summary>
        public string ActiveViewType
        {
            get
            {
                if (m_IsMapCtrlactive)
                    return "MapControl";
                else
                    return "PageLayoutControl";
            }
        }
        /// <summary>
        /// ȡ�õ�ǰ���Control
        /// </summary>
        public object ActiveControl
        {
            get
            {
                if (m_mapControl == null || m_pageLayoutControl == null)
                    throw new Exception("ControlsSynchronizer::ActiveControl:\r\nEither MapControl or PageLayoutControl are not initialized!");
                if (m_IsMapCtrlactive)
                    return m_mapControl.Object;
                else
                    return m_pageLayoutControl.Object;
            }
        }
        #endregion


        #region Methods
        /// <summary>
        /// ����MapControl�����the PagleLayoutControl
        /// </summary>
        public void ActivateMap()
        {
            try
            {
                if (m_pageLayoutControl == null || m_mapControl == null)
                    throw new Exception("ControlsSynchronizer::ActivateMap:\r\nEither MapControl or PageLayoutControl are not initialized!");
                //���浱ǰPageLayout��CurrentTool
                if (m_pageLayoutControl.CurrentTool != null) m_pageLayoutActiveTool = m_pageLayoutControl.CurrentTool;
                //���PagleLayout
                m_pageLayoutControl.ActiveView.Deactivate();
                //����MapControl
                m_mapControl.ActiveView.Activate(m_mapControl.hWnd);
                //��֮ǰMapControl���ʹ�õ�tool����Ϊ���tool������MapControl��CurrentTool
                if (m_mapActiveTool != null) m_mapControl.CurrentTool = m_mapActiveTool;
                m_IsMapCtrlactive = true;
                //Ϊÿһ����framework controls,����Buddy controlΪMapControl
                this.SetBuddies(m_mapControl.Object);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::ActivateMap:\r\n{0}", ex.Message));
            }
        }
        /// <summary>
        /// ����PagleLayoutControl������MapCotrol
        /// </summary>
        public void ActivatePageLayout()
        {
            try
            {
                if (m_pageLayoutControl == null || m_mapControl == null)
                    throw new Exception("ControlsSynchronizer::ActivatePageLayout:\r\nEither MapControl or PageLayoutControl are not initialized!");
                //���浱ǰMapControl��CurrentTool
                if (m_mapControl.CurrentTool != null) m_mapActiveTool = m_mapControl.CurrentTool;
                //���MapControl
                m_mapControl.ActiveView.Deactivate();
                //����PageLayoutControl
                m_pageLayoutControl.ActiveView.Activate(m_pageLayoutControl.hWnd);
                //��֮ǰPageLayoutControl���ʹ�õ�tool����Ϊ���tool������PageLayoutControl��CurrentTool
                if (m_pageLayoutActiveTool != null) m_pageLayoutControl.CurrentTool = m_pageLayoutActiveTool;
                m_IsMapCtrlactive = false;
                //Ϊÿһ����framework controls,����Buddy controlΪPageLayoutControl
                this.SetBuddies(m_pageLayoutControl.Object);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::ActivatePageLayout:\r\n{0}", ex.Message));
            }
        }
        /// <summary>
        /// ����һ����ͼ, �û�PageLayoutControl��MapControl��focus map
        /// </summary>
        /// <param name="newMap"></param>
        public void ReplaceMap(IMap newMap)
        {
            if (newMap == null)
                throw new Exception("ControlsSynchronizer::ReplaceMap:\r\nNew map for replacement is not initialized!");
            if (m_pageLayoutControl == null || m_mapControl == null)
                throw new Exception("ControlsSynchronizer::ReplaceMap:\r\nEither MapControl or PageLayoutControl are not initialized!");
            //create a new instance of IMaps collection which is needed by the PageLayout
            //����һ��PageLayout��Ҫ�õ���,�µ�IMaps collection��ʵ��
            IMaps maps = new Maps();
            //add the new map to the Maps collection
            //���µĵ�ͼ�ӵ�Maps collection��ͷȥ
            maps.Add(newMap);
            bool bIsMapActive = m_IsMapCtrlactive;
            //call replace map on the PageLayout in order to replace the focus map
            //we must call ActivatePageLayout, since it is the control we call 'ReplaceMaps'
            //����PageLayout��replace map���û�focus map
            //���Ǳ������ActivatePageLayout,��Ϊ�����Ǹ����ǿ��Ե���"ReplaceMaps"��Control
            this.ActivatePageLayout();
            m_pageLayoutControl.PageLayout.ReplaceMaps(maps);
            //assign the new map to the MapControl
            //���µĵ�ͼ����MapControl
            m_mapControl.Map = newMap;
            //reset the active tools
            //����active tools
            m_pageLayoutActiveTool = null;
            m_mapActiveTool = null;
            //make sure that the last active control is activated
            //ȷ��֮ǰ���control������
            if (bIsMapActive)
            {
                this.ActivateMap();
                m_mapControl.ActiveView.Refresh();
            }
            else
            {
                this.ActivatePageLayout();
                m_pageLayoutControl.ActiveView.Refresh();
            }
        }
        /// <summary>
        /// bind the MapControl and PageLayoutControl together by assigning a new joint focus map
        /// ָ����ͬ��Map����MapControl��PageLayoutControl����һ��
        /// </summary>
        /// <param name="mapControl"></param>
        /// <param name="pageLayoutControl"></param>
        /// <param name="activateMapFirst">true if the MapControl supposed to be activated first,���MapControl�����ȼ���,��Ϊtrue</param>
        public void BindControls(IMapControl4 mapControl, IPageLayoutControl2 pageLayoutControl, bool activateMapFirst)
        {
            if (mapControl == null || pageLayoutControl == null)
                throw new Exception("ControlsSynchronizer::BindControls:\r\nEither MapControl or PageLayoutControl are not initialized!");
            m_mapControl = MapControl;
            m_pageLayoutControl = pageLayoutControl;
            this.BindControls(activateMapFirst);
        }
        /// <summary>
        /// bind the MapControl and PageLayoutControl together by assigning a new joint focus map
        /// ָ����ͬ��Map����MapControl��PageLayoutControl����һ��
        /// </summary>
        /// <param name="activateMapFirst">true if the MapControl supposed to be activated first,���MapControl�����ȼ���,��Ϊtrue</param>
        public void BindControls(bool activateMapFirst)
        {
            if (m_pageLayoutControl == null || m_mapControl == null)
                throw new Exception("ControlsSynchronizer::BindControls:\r\nEither MapControl or PageLayoutControl are not initialized!");
            //create a new instance of IMap
            //����IMap��һ��ʵ��
            IMap newMap = new MapClass();
            newMap.Name = "Map";
            //create a new instance of IMaps collection which is needed by the PageLayout
            //����һ���µ�IMaps collection��ʵ��,����PageLayout����Ҫ��
            IMaps maps = new Maps();
            //add the new Map instance to the Maps collection
            //���µ�Mapʵ������Maps collection
            maps.Add(newMap);
            //call replace map on the PageLayout in order to replace the focus map
            //����PageLayout��replace map���û�focus map
            m_pageLayoutControl.PageLayout.ReplaceMaps(maps);
            //assign the new map to the MapControl
            //���µ�map����MapControl
            m_mapControl.Map = newMap;
            //reset the active tools
            //����active tools
            m_pageLayoutActiveTool = null;
            m_mapActiveTool = null;
            //make sure that the last active control is activated
            //ȷ�������control������
            if (activateMapFirst)
                this.ActivateMap();
            else
                this.ActivatePageLayout();
        }
        /// <summary>
        ///by passing the application's toolbars and TOC to the synchronization class, it saves you the
        ///management of the buddy control each time the active control changes. This method ads the framework
        ///control to an array; once the active control changes, the class iterates through the array and
        ///calles SetBuddyControl on each of the stored framework control.
        /// </summary>
        /// <param name="control"></param>
        public void AddFrameworkControl(object control)
        {
            if (control == null)
                throw new Exception("ControlsSynchronizer::AddFrameworkControl:\r\nAdded control is not initialized!");
            m_frameworkControls.Add(control);
        }
        /// <summary>
        /// Remove a framework control from the managed list of controls
        /// </summary>
        /// <param name="control"></param>
        public void RemoveFrameworkControl(object control)
        {
            if (control == null)
                throw new Exception("ControlsSynchronizer::RemoveFrameworkControl:\r\nControl to be removed is not initialized!");
            m_frameworkControls.Remove(control);
        }
        /// <summary>
        /// Remove a framework control from the managed list of controls by specifying its index in the list
        /// </summary>
        /// <param name="index"></param>
        public void RemoveFrameworkControlAt(int index)
        {
            if (m_frameworkControls.Count < index)
                throw new Exception("ControlsSynchronizer::RemoveFrameworkControlAt:\r\nIndex is out of range!");
            m_frameworkControls.RemoveAt(index);
        }
        /// <summary>
        /// when the active control changes, the class iterates through the array of the framework controls
        /// and calles SetBuddyControl on each of the controls.
        /// </summary>
        /// <param name="buddy">the active control</param>
        private void SetBuddies(object buddy)
        {
            try
            {
                if (buddy == null)
                    throw new Exception("ControlsSynchronizer::SetBuddies:\r\nTarget Buddy Control is not initialized!");
                foreach (object obj in m_frameworkControls)
                {
                    if (obj is IToolbarControl)
                    {
                        ((IToolbarControl)obj).SetBuddyControl(buddy);
                    }
                    else if (obj is ITOCControl)
                    {
                        ((ITOCControl)obj).SetBuddyControl(buddy);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::SetBuddies:\r\n{0}", ex.Message));
            }
        }
        #endregion
    }
}
