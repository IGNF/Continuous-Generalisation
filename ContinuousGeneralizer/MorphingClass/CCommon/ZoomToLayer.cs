//�Ŵ�������ͼ����ZoomToLayer��
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace MorphingClass.CCommon
{
    /// <summary>
    /// �Ŵ�������ͼ��
    /// </summary>
    public sealed class ZoomToLayer : BaseCommand
    {
        private IMapControl4 m_mapControl;
        public ZoomToLayer()
        {
            base.m_caption = "Zoom To Layer";
        }
        public override void OnClick()
        {
            ILayer layer = (ILayer)m_mapControl.CustomProperty;
            m_mapControl.Extent = layer.AreaOfInterest;
        }
        public override void OnCreate(object hook)
        {
            m_mapControl = (IMapControl4)hook;
        }
    }
}
