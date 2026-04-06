async function completeModule(moduleId, moduleKey) {
    localStorage.setItem(moduleKey, "completed");
    try {
        await fetch(`/Modules/SaveProgress?moduleId=${moduleId}&status=completed`, {
            method: 'POST'
        });
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
  const basicsDone =
    localStorage.getItem("module_devopsBasics") === "completed";
  const dockerDone = localStorage.getItem("module_docker") === "completed";

  const dockerCard = document.getElementById("card-docker");
  const dockerBtn = document.getElementById("btn-docker");
  const dockerBadge = document.getElementById("status-docker");

  // unlock docker if basics done
  if (dockerCard && dockerBtn) {
    if (basicsDone) {
      if (localStorage.getItem("module_docker") !== "completed") {
        dockerCard.classList.remove("locked");
        dockerCard.classList.add("in-progress");
        if (dockerBadge) {
          dockerBadge.innerText = "Available";
        }
        dockerBtn.innerText = "Start Module";
      } else {
        dockerCard.classList.remove("locked", "in-progress");
        dockerCard.classList.add("completed");
        if (dockerBadge) {
          dockerBadge.innerText = "Completed";
        }
        dockerBtn.innerText = "Repeat";
      }

      dockerBtn.onclick = null;
    } else {
      dockerBtn.onclick = function (e) {
        e.preventDefault();
        alert("Please complete 'DevOps Fundamentals' first!");
      };
    }
  }

  // unlock cicd if docker is done
  const cicdCard = document.getElementById("card-cicd");
  const cicdBtn = document.getElementById("btn-cicd");
  const cicdBadge = document.getElementById("status-cicd");

  if (cicdCard && cicdBtn) {
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
}

async function syncProgressWithDatabase() {
    try {
        const response = await fetch('/Modules/GetUserProgress'); 
        const data = await response.json();

        
        data.forEach(p => {
            
            let key = "";
            if (p.moduleId === 1) key = "module_devopsBasics";
            if (p.moduleId === 2) key = "module_cicd";
            if (p.moduleId === 3) key = "module_docker";

            localStorage.setItem(key, p.status);
        });

        
        updateProgress();
    } catch (e) {
        
        updateProgress();
    }
}


window.onload = syncProgressWithDatabase;
function resetEverything() {
  localStorage.clear();
  location.reload();
}
