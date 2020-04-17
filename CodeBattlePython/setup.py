from distutils.core import setup

setup(name='bombermanclient',
      version='1.0',
      description='Bomberman Dojo game client',
      author='',
      author_email='',
      packages=['bombermanclient'],
      install_requires=['websocket-client', 'click'],
      entry_points={
          'console_scripts': [
              'bombermanclient=bombermanclient.CodeBattlePython:main']})
