﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;

namespace Paint
{
    class MainViewModel:BaseViewModel
    {
        enum Sections
        {
            Painting,
            Undoing,
            Redoing,
        }
        //Properties
        private LinkedList<StrokeCollection> _Undo;
        public LinkedList<StrokeCollection> Undo {
            get => _Undo;
            set
            {
                _Undo = value;
                OnPropertyChanged("Undo");
            }
        }
        private LinkedList<StrokeCollection> _Redo;
        public LinkedList<StrokeCollection> Redo
        {
            get => _Redo;
            set
            {
                _Redo = value;
                OnPropertyChanged("Redo");
            }
        }

        private StrokeCollection _InkStrokes;
        public StrokeCollection InkStrokes
        {
            get => _InkStrokes;
            set
            {
                _InkStrokes = value;
                OnPropertyChanged("StrokesChanged");
            }
        }

        private Sections _section;

        //Command
        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }
        public MainViewModel()
        {
            Undo = new LinkedList<StrokeCollection>();
            Redo = new LinkedList<StrokeCollection>();
            InkStrokes = new StrokeCollection();
            _section = Sections.Painting;

            //Undo command Implementation
            UndoCommand = new RelayCommand<object>((p) =>
            {
                if (Undo.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                StrokeCollection _undo = Undo.First();
                _section = Sections.Undoing;
                InkStrokes.Remove(_undo);
                Undo.RemoveFirst();
                Redo.AddFirst(_undo);
            });

            //Redo command Implementation
            RedoCommand = new RelayCommand<object>((p) =>
            {
                if (Redo.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                StrokeCollection _redo = Redo.First();
                _section = Sections.Redoing;
                InkStrokes.Add(_redo);
                Redo.RemoveFirst();
                Undo.AddFirst(_redo);
            });
        }

        public void StrokesChanged(StrokeCollection strokes)
        {
            if(_section != Sections.Painting)
            {
                _section = Sections.Painting;
                return;
            }
            Undo.AddFirst(strokes);
            Redo.Clear();
        }
    }
}
