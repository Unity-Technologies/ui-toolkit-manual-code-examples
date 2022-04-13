using UnityEngine;
using UnityEngine.UIElements;

namespace MyUILibrary
{
    public class EllipseMesh
    {
        int m_NumSteps;
        float m_Width;
        float m_Height;
        Color m_Color;
        float m_BorderSize;
        bool m_IsDirty;
        public Vertex[] vertices { get; private set; }
        public ushort[] indices { get; private set; }

        public EllipseMesh(int numSteps)
        {
            m_NumSteps = numSteps;
            m_IsDirty = true;
        }

        public void UpdateMesh()
        {
            if (!m_IsDirty)
                return;

            int numVertices = numSteps * 2;
            int numIndices = numVertices * 6;

            if (vertices == null || vertices.Length != numVertices)
                vertices = new Vertex[numVertices];

            if (indices == null || indices.Length != numIndices)
                indices = new ushort[numIndices];

            float stepSize = 360.0f / (float)numSteps;
            float angle = -180.0f;

            for (int i = 0; i < numSteps; ++i)
            {
                angle -= stepSize;
                float radians = Mathf.Deg2Rad * angle;

                float outerX = Mathf.Sin(radians) * width;
                float outerY = Mathf.Cos(radians) * height;
                Vertex outerVertex = new Vertex();
                outerVertex.position = new Vector3(width + outerX, height + outerY, Vertex.nearZ);
                outerVertex.tint = color;
                vertices[i * 2] = outerVertex;

                float innerX = Mathf.Sin(radians) * (width - borderSize);
                float innerY = Mathf.Cos(radians) * (height - borderSize);
                Vertex innerVertex = new Vertex();
                innerVertex.position = new Vector3(width + innerX, height + innerY, Vertex.nearZ);
                innerVertex.tint = color;
                vertices[i * 2 + 1] = innerVertex;

                indices[i * 6] = (ushort)((i == 0) ? vertices.Length - 2 : (i - 1) * 2); // previous outer vertex
                indices[i * 6 + 1] = (ushort)(i * 2); // current outer vertex
                indices[i * 6 + 2] = (ushort)(i * 2 + 1); // current inner vertex

                indices[i * 6 + 3] = (ushort)((i == 0) ? vertices.Length - 2 : (i - 1) * 2); // previous outer vertex
                indices[i * 6 + 4] = (ushort)(i * 2 + 1); // current inner vertex
                indices[i * 6 + 5] = (ushort)((i == 0) ? vertices.Length - 1 : (i - 1) * 2 + 1); // previous inner vertex
            }

            m_IsDirty = false;
        }

        public bool isDirty => m_IsDirty;

        void CompareAndWrite(ref float field, float newValue)
        {
            if (Mathf.Abs(field - newValue) > float.Epsilon)
            {
                m_IsDirty = true;
                field = newValue;
            }
        }

        public int numSteps
        {
            get => m_NumSteps;
            set
            {
                m_IsDirty = value != m_NumSteps;
                m_NumSteps = value;
            }
        }

        public float width
        {
            get => m_Width;
            set => CompareAndWrite(ref m_Width, value);
        }

        public float height
        {
            get => m_Height;
            set => CompareAndWrite(ref m_Height, value);
        }

        public Color color
        {
            get => m_Color;
            set
            {
                m_IsDirty = value != m_Color;
                m_Color = value;
            }
        }

        public float borderSize
        {
            get => m_BorderSize;
            set => CompareAndWrite(ref m_BorderSize, value);
        }

    }
}