var BombermanBlocks =
    {
        Unknown: 0,

        Bomberman: '☺',
        BombBomberman: '☻',
        DeadBomberman: 'Ѡ',

        OtherBomberman: '♥',
        OtherBombBomberman: '♠',
        OtherDeadBomberman: '♣',

        BombTimer5: '5',
        BombTimer4: '4',
        BombTimer3: '3',
        BombTimer2: '2',
        BombTimer1: '1',
        Boom: '҉',

        Wall: '☼',
        WallDestroyable: '#',
        DestroyedWall: 'H',

        MeatChopper: '&',
        DeadMeatChopper: 'x',

        Space: ' '
    };

var BombAction =
    {
        None: 0,
        BeforeTurn: 1,
        AfterTurn: 2
    };

class GameClient {
    constructor(server, user, code) {
        this.path = "ws://" + server + "/codenjoy-contest/ws?user=" + user + "&code=" + code
    }

    run(callback) {
        this.socket = new WebSocket(this.path);
        this.socket.onopen = this.onOpen;
        this.socket.onerror = this.onError;
        this.socket.onclose = this.onClose;
        this.socket.onmessage = function (event) {
            var data = event.data.substring(6);
            this.size = Math.sqrt(data.length);
            var currentChar = 0;

            this.map = [];
            for (var j = 0; j < this.size; j++) {
                this.map[j] = [];
                for (var i = 0; i < this.size; i++) {
                    for (var key in BombermanBlocks) {
                        if (data[currentChar] == BombermanBlocks[key]) {
                            this.map[j][i] = BombermanBlocks[key];
                            if (this.map[j][i] == BombermanBlocks.Bomberman ||
                                this.map[j][i] == BombermanBlocks.BombBomberman ||
                                this.map[j][i] == BombermanBlocks.DeadBomberman) {
                                this.playerX = i;
                                this.playerY = j;
                            }
                        }
                    }
                    currentChar++;
                }
            }

            callback();
        }
    }

    get size() {
        return this.socket.size;
    }

    get map() {
        return this.socket.map;
    }

    get playerX() {
        return this.socket.playerX;
    }

    get playerY() {
        return this.socket.playerY;
    }

    set textArea(text) {
        this.text = text
    }

    onOpen() {
        text.value += "Connection established\n";
    }

    onClose(event) {
        if (event.wasClean) {
            text.value += "### disconnected ###\n"
        } else {
            text.value += "### accidentally disconnected ###\n";
            text.value += " - Err code: " + event.code + ", Reason: " + event.reason + "\n";
        }
    }

    onError(error) {
        text.value += "### error ###\n" + error.message + "\n";
    }

    send(msg) {
        text.value += "Sending: " + msg + '\n'
        this.socket.send(msg)
    }

    up(action = BombAction.None) {
        this.send((action == BombAction.BeforeTurn ? "ACT," : "") + "UP" + (action == BombAction.AfterTurn ? ",ACT" : ""));
    }

    down(action = BombAction.None) {
        this.send((action == BombAction.BeforeTurn ? "ACT," : "") + "DOWN" + (action == BombAction.AfterTurn ? ",ACT" : ""));
    }

    right(action = BombAction.None) {
        this.send((action == BombAction.BeforeTurn ? "ACT," : "") + "RIGHT" + (action == BombAction.AfterTurn ? ",ACT" : ""));
    }

    left(action = BombAction.None) {
        this.send((action == BombAction.BeforeTurn ? "ACT," : "") + "LEFT" + (action == BombAction.AfterTurn ? ",ACT" : ""));
    }

    act() {
        this.send("ACT");
    }

    blank() {
        this.send("");
    }
}
