/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 - 2020 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
var D = function (name, next) {

    var toString = function () {
        var result = name;
        if(next) result = result + ', ' + next.toString();
        return result;
    };

    return {
        toString: toString,
    };
};

var Direction = {
    UP : D('up'),                 // you can move
    DOWN : D('down'),
    LEFT : D('left'),
    RIGHT : D('right'),
    ACT : D('act'),                // drop bomb
    STOP : D(''),                  // stay
    BOMBANDUP : D('act', 'up'),
    BOMBANDLEFT : D('act', 'left'),
    BOMBANDDOWN : D('act', 'down'),
    BOMBANDRIGHT : D('act', 'right'),                  
};

Direction.values = function () {
    return [Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.ACT, Direction.STOP, Direction.BOMBANDUP, Direction.BOMBANDLEFT, Direction.BOMBANDDOWN, Direction.BOMBANDRIGHT];
};

Direction.valueOf = function (index) {
    var directions = Direction.values();
    for (var i in directions) {
        var direction = directions[i];
        if (direction.getIndex() == index) {
            return direction;
        }
    }
    return Direction.STOP;
};


if (module) module.exports = Direction;
