from distutils.core import setup

setup(name='codebattleclient',
      version='1.0',
      description='CodeBattle Client game client',
      author='',
      author_email='',
      packages=['codebattleclient'],
      install_requires=['websocket-client', 'click'],
      entry_points={'console_scripts': ['codebattleclient=codebattleclient.Main:main']})
