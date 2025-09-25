import os
import sys
sys.path.insert(0, os.path.abspath('../../'))  # so autodoc can see your code

project = 'EtheriumLib'
author = 'LostGameDev'
release = '1.0.0'

extensions = [
    'sphinx.ext.autodoc',
    'sphinx.ext.napoleon',  # Google/Numpy style docstrings
    'myst_parser',          # Markdown support
]

source_suffix = {
    '.rst': 'restructuredtext',
    '.md': 'markdown',
}

html_theme = 'sphinx_rtd_theme'
