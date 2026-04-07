async function completeModule(moduleId, moduleKey) {
    try {
        await fetch(`/Modules/SaveProgress?moduleId=${moduleId}&status=completed`, {
            method: 'POST'
        });
        localStorage.setItem(moduleKey, "completed");
    } catch (err) {
        console.error("Database sync failed", err);
    }

    alert("Module finished! Progress saved.");
    window.location.href = "/";
}

function updateProgress() {
  let completedModules = 0;
  const totalModules = 3;

  if (localStorage.getItem("module_docker") === "completed") {
    completedModules++;
    UpdateProgressBadge(true, "status-docker");
  }

  if (localStorage.getItem("module_cicd") === "completed") {
    completedModules++;
    UpdateProgressBadge(true, "status-cicd");
  }

  if (localStorage.getItem("module_devopsBasics") === "completed") {
    completedModules++;
    UpdateProgressBadge(true, "status-devopsBasics");
  }

  const progressBar = document.querySelector(".progress-bar");
  if (progressBar) {
    let percent = (completedModules / totalModules) * 100;
    progressBar.style.width = percent + "%";
    progressBar.innerText = Math.round(percent) + "%";
  }
  const progressText = document.getElementById("progress-text");
  if (progressText) {
    progressText.innerText = `You have completed ${completedModules} out of ${totalModules} modules.`;
  }
}

window.onload = updateProgress;

function UpdateProgressBadge(completed, badgeId) {
  const progressBadge = document.getElementById(badgeId);
  if (progressBadge) {
    if (completed) {
      progressBadge.innerText = "Completed";
      const card = progressBadge.closest(".module-card");
      if (card) {
        card.classList.add("completed");
        card.classList.remove("in-progress");

        const btn = card.querySelector(".btn");
        if (btn) btn.innerText = "Repeat";
      }
    }
  }
  handleLocking();
}

function handleLocking() {
    // get status of all modules from localStorage
    const basicsStatus = localStorage.getItem("module_devopsBasics");
    const dockerStatus = localStorage.getItem("module_docker");
    const cicdStatus = localStorage.getItem("module_cicd");

    // helper variables
    const basicsDone = basicsStatus === "completed";
    const dockerDone = dockerStatus === "completed";
    const cicdDone = cicdStatus === "completed";

    // module devops basics
    const basicsCard = document.getElementById("card-devopsbasics");
    const basicsBtn = document.getElementById("btn-devopsbasics");
    const basicsBadge = document.getElementById("status-devopsbasics");

    // module DevOps basics always open and available
    if (basicsCard && basicsBtn) {
        if (basicsDone) {
            setModuleState(basicsCard, basicsBtn, basicsBadge, "completed");
        } else {
            setModuleState(basicsCard, basicsBtn, basicsBadge, "available");
        }
    }
    // Module Docker dependant on DevOps basics
    const dockerCard = document.getElementById("card-docker");
    const dockerBtn = document.getElementById("btn-docker");
    const dockerBadge = document.getElementById("status-docker");

    if (dockerCard && dockerBtn) {
        if (dockerDone) {
            setModuleState(dockerCard, dockerBtn, dockerBadge, "completed");
        } else if (basicsDone) {
            setModuleState(dockerCard, dockerBtn, dockerBadge, "available");
        } else {
            setModuleState(dockerCard, dockerBtn, dockerBadge, "locked", "Please complete 'DevOps Fundamentals' first!");
        }
    }

    // Module CI/CD dependant on Docker
    const cicdCard = document.getElementById("card-cicd");
    const cicdBtn = document.getElementById("btn-cicd");
    const cicdBadge = document.getElementById("status-cicd");

    if (cicdCard && cicdBtn) {
        if (cicdDone) {
            setModuleState(cicdCard, cicdBtn, cicdBadge, "completed");
        } else if (dockerDone) {
            setModuleState(cicdCard, cicdBtn, cicdBadge, "available");
        } else {
            setModuleState(cicdCard, cicdBtn, cicdBadge, "locked", "Please complete the 'Docker' module first!");
        }
    }
}

function setModuleState(card, btn, badge, state, lockMessage = "") {
    // Reset classes
    card.classList.remove("locked", "in-progress", "completed");
    btn.onclick = null;

    if (state === "completed") {
        card.classList.add("completed");
        if (badge) badge.innerText = "Completed";
        btn.innerText = "Repeat Module";
    }
    else if (state === "available") {
        card.classList.add("in-progress");
        if (badge) badge.innerText = "Available";
        btn.innerText = "Start Module";
    }
    else if (state === "locked") {
        card.classList.add("locked");
        if (badge) badge.innerText = "Locked";
        btn.innerText = "Locked";
        btn.onclick = function (e) {
            e.preventDefault();
            alert(lockMessage);
        };
    }
}

  // unlock cicd if docker is done
  const cicdCard = document.getElementById("card-cicd");
  const cicdBtn = document.getElementById("btn-cicd");
  const cicdBadge = document.getElementById("status-cicd");

if (cicdCard && cicdBtn)
{
    if (dockerDone) {
      if (localStorage.getItem("module_cicd") !== "completed") {
        cicdCard.classList.remove("locked");
        cicdCard.classList.add("in-progress");
        if (cicdBadge) {
          cicdBadge.innerText = "Available";
        }
        cicdBtn.innerText = "Start Module";
      } else {
        cicdCard.classList.remove("locked", "in-progress");
        cicdCard.classList.add("completed");
        if (cicdBadge) {
          cicdBadge.innerText = "Completed";
        }
        cicdBtn.innerText = "Repeat";
      }

      cicdBtn.onclick = null;
    } else {
      cicdBtn.onclick = function (e) {
        e.preventDefault();
        alert("Please complete the 'Docker' module first!");
      };
    }
  
}

async function syncProgressWithDatabase() {
    try {
        const response = await fetch('/Modules/GetUserProgress');
        const data = await response.json();

        localStorage.clear();

        data.forEach(p => {
            let key = "";
            if (p.moduleId === 1) key = "module_devopsBasics";
            if (p.moduleId === 2) key = "module_docker"; 
            if (p.moduleId === 3) key = "module_cicd";  

            localStorage.setItem(key, p.status);
        });

        updateProgress(); 
    } catch (e) {
        console.error("Sync failed", e);
        updateProgress();
    }
}

window.onload = syncProgressWithDatabase;


window.resetEverything = async function () {
    console.log("Reset function called"); 

    if (confirm("Are you sure you want to reset all progress?")) {
        try {
            const response = await fetch('/Modules/ClearProgress', {
                method: 'POST'
            });

            if (response.ok) {
                localStorage.clear();
                alert("Progress has been reset successfully!");
                window.location.href = "/"; 
            } else {
                alert("Server error during reset. Status: " + response.status);
            }
        } catch (err) {
            console.error("Fetch error:", err);
            alert("Connection to server failed.");
        }
    }
};