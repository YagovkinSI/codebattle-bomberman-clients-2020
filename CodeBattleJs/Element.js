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
var Element = {

   /// This is your Bomberman
   BOMBERMAN : '☺',             // this is what he usually looks like
   BOMB_BOMBERMAN : '☻',        // this is if he is sitting on own bomb
   DEAD_BOMBERMAN : 'Ѡ',        // oops, your Bomberman is dead (don't worry, he will appear somewhere in next move)

   /// this is other players Bombermans
   OTHER_BOMBERMAN : '♥',       // this is what other Bombermans looks like
   OTHER_BOMB_BOMBERMAN : '♠',  // this is if player just set the bomb
   OTHER_DEAD_BOMBERMAN : '♣',  // enemy corpse (it will disappear shortly, right on the next move)

   /// the bombs
   BOMB_TIMER_5 : '5',          // after bomberman set the bomb, the timer starts (5 tacts)
   BOMB_TIMER_4 : '4',          // this will blow up after 4 tacts
   BOMB_TIMER_3 : '3',          // this after 3
   BOMB_TIMER_2 : '2',          // two
   BOMB_TIMER_1 : '1',          // one
   BOOM : '҉',                  // Boom! this is what is bomb does, everything that is destroyable got destroyed

   /// walls
   WALL : '☼',                  // indestructible wall - it will not fall from bomb
   DESTROYABLE_WALL : '#',      // this wall could be blowed up
   DESTROYED_WALL : 'H',        // this is how broken wall looks like, it will dissapear on next move

   /// meatchoppers
   MEAT_CHOPPER : '&',          // this guys runs over the board randomly and gets in the way all the time
                                // if it will touch bomberman - it will die
   DEAD_MEAT_CHOPPER : 'x',     // this is chopper corpse

   /// a void
   NONE : ' '                  // this is the only place where you can move your Bomberman

};

if(module) module.exports = Element;

