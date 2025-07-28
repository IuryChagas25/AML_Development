using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Engine.CAEX;

namespace WindowsFormsApplication1.Model
{
    public class CAEXService : ICAEXService
    {
        public string ShowOpenDialog()
        {
            using var dlg = new OpenFileDialog { Filter = "Arquivos AML|*.aml;*.xml", Title = "Selecione um arquivo AML/CAEX" };
            return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
        }
        public string ShowSaveDialog()
        {
            using var dlg = new SaveFileDialog { Filter = "Arquivos AML|*.aml", Title = "Salvar arquivo AML/CAEX" };
            return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
        }
        public Task<CAEXDocument> LoadDocumentAsync(string path, CancellationToken token)
            => Task.Run(() => { token.ThrowIfCancellationRequested(); return CAEXDocument.LoadFromFile(path); }, token);
        public Task SaveDocumentAsync(CAEXDocument doc, string path, CancellationToken token)
            => Task.Run(() => { token.ThrowIfCancellationRequested(); doc.SaveToFile(path, true); }, token);
        public Task<(bool IsValid, string[] Errors)> ValidateDocumentAsync(CAEXDocument doc, CancellationToken token)
            => Task.Run(() => { token.ThrowIfCancellationRequested(); bool valid = doc.Validate(out var errors); return (valid, errors); }, token);
        public IEnumerable<TreeNode> BuildTreeNodes(CAEXDocument doc)
        {
            var nodes = new List<TreeNode>();
            foreach (var ih in doc.CAEXFile.InstanceHierarchy)
            {
                var n = new TreeNode(ih.Name) { Tag = ih };
                BuildInternalElements(n, ih.InternalElement);
                nodes.Add(n);
            }
            foreach (var rl in doc.CAEXFile.RoleClassLib)
            {
                var n = new TreeNode(rl.Name) { Tag = rl };
                BuildRoleClasses(n, rl.RoleClass);
                nodes.Add(n);
            }
            foreach (var sucl in doc.CAEXFile.SystemUnitClassLib)
            {
                var n = new TreeNode(sucl.Name) { Tag = sucl };
                BuildSystemUnits(n, sucl.SystemUnitClass);
                nodes.Add(n);
            }
            return nodes;
        }
        public void EditNodeValue(CAEXDocument doc, TreeNode node, string newValue)
        {
            switch (node.Tag)
            {
                case AttributeType attr:
                    attr.Value = newValue;
                    node.Text = FormatAttribute(attr);
                    break;
                case InstanceHierarchyType ih:
                    ih.Name = newValue;
                    node.Text = ih.Name;
                    break;
                case InternalElementType ie:
                    ie.Name = newValue;
                    node.Text = ie.Name;
                    break;
                case RoleFamilyType rf:
                    rf.Name = newValue;
                    node.Text = rf.Name;
                    break;
                case SystemUnitFamilyType su:
                    su.Name = newValue;
                    node.Text = su.Name;
                    break;
                default:
                    throw new InvalidOperationException("Tipo de nó não suportado para edição.");
            }
        }
        private string FormatAttribute(AttributeType attr)
        {
            var unit = string.IsNullOrEmpty(attr.Unit) ? string.Empty : $" [{attr.Unit}]";
            var val = attr.Value ?? "<sem valor>";
            return $"{attr.Name}: {val} ({attr.AttributeDataType}){unit}";
        }
        private void BuildAttributes(TreeNode parent, IEnumerable<AttributeType> attrs)
        {
            foreach (var a in attrs)
            {
                var n = new TreeNode(FormatAttribute(a)) { Tag = a };
                parent.Nodes.Add(n);
                if (a.Attribute != null) BuildAttributes(n, a.Attribute);
            }
        }
        private void BuildInternalElements(TreeNode parent, IEnumerable<InternalElementType> elems)
        {
            foreach (var ie in elems)
            {
                var n = new TreeNode(ie.Name) { Tag = ie };
                parent.Nodes.Add(n);
                BuildAttributes(n, ie.Attribute);
                BuildInternalElements(n, ie.InternalElement);
            }
        }
        private void BuildRoleClasses(TreeNode parent, IEnumerable<RoleFamilyType> classes)
        {
            foreach (var rc in classes)
            {
                var n = new TreeNode(rc.Name) { Tag = rc };
                parent.Nodes.Add(n);
                BuildAttributes(n, rc.Attribute);
                BuildRoleClasses(n, rc.RoleClass);
            }
        }
        private void BuildSystemUnits(TreeNode parent, IEnumerable<SystemUnitFamilyType> units)
        {
            foreach (var su in units)
            {
                var n = new TreeNode(su.Name) { Tag = su };
                parent.Nodes.Add(n);
                BuildAttributes(n, su.Attribute);
                BuildInternalElements(n, su.InternalElement);
                BuildSystemUnits(n, su.SystemUnitClass);
            }
        }
    }
}