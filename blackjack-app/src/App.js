import React, { useState } from 'react';
import './App.css';

const API_BASE = "http://localhost:5187/api/blackjack"; // Your backend port

function App() {
  const [userId, setUserId] = useState(0);
  const [bet, setBet] = useState(0);
  const [game, setGame] = useState(null);

  const startGame = async () => {
    try {
      const res = await fetch(`${API_BASE}/start?userId=${userId}&bet=${bet}`, {
        method: 'POST',
      });

      if (!res.ok) {
        const errorText = await res.text(); // or res.json() if your API returns structured error
        throw new Error(`Failed to start game: ${errorText}`);
      }

      const data = await res.json();
      setGame(data);
    } catch (error) {
      console.error("Start Game Error:", error);
      alert(error.message); // or use a nicer toast/snackbar
    }
  };
  
  const hit = async () => {
    const res = await fetch(`${API_BASE}/hit?userId=${userId}`, { method: 'POST' });
    const data = await res.json();
    console.log("Hit action result:", data); // Debugging line to check game state
    setGame(data);
  };

  const stand = async () => {
    const res = await fetch(`${API_BASE}/stand?userId=${userId}`, { method: 'POST' });
    const data = await res.json();
    console.log("Stand action result:", data); // Debugging line to check game state
    setGame(data);
  };

  return (
      <div className="App">
        <h1>🃏 Blackjack Game</h1>
        <div>
          <label>User ID:</label>
          <input value={userId} onChange={(e) => setUserId(e.target.value)} />
          <label>Bet:</label>
          <input value={bet} onChange={(e) => setBet(e.target.value)} />
          <button onClick={startGame}>Start Game</button>
        </div>

        {game && (
            <>
              <h2>Player Hand ({game.playerValue}):</h2>
              <p>{game.playerHand.join(', ')}</p>

              <h2>Dealer Hand ({game.dealerValue}):</h2>
              <p>{game.dealerHand.join(', ')}</p>
            </>
        )}
        
        {game && game.isGameOver ? (
            <>
              <h2>Result: {game.gameResult ? game.gameResult : 'No result yet'}</h2>
              <button onClick={startGame}>Play Again</button>
            </>
        ) : (
            <>
              <div>
                <button onClick={hit}>Hit</button>
                <button onClick={stand}>Stand</button>
              </div>
            </>
        )}
      </div>
  );
}

export default App;
